using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MobileProject.Models;
using System.Collections.ObjectModel;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using Windows.UI.Popups;

namespace MobileProject
{
    public sealed partial class MainPage : Page
    {
        private const string JSONFILENAME = "data.json";

        private ObservableCollection<Entry> entryList;
        private static List<Entry> entryListFilter = new List<Entry>();

        private string listBoxID = "Home";
        private string latitudecord;
        private string longitudecord;
        private string currentTime;
        private string currentDate;
        private string textToSpeech;

        private int index;
        private int entryListFilterCount;
        private int entryCount;
        private static int indexAt;

        public MainPage()
        {
            InitializeComponent();
            entryList = new ObservableCollection<Entry>();
            ItemsView.ItemsSource = entryList;
            Application.Current.Suspending += new SuspendingEventHandler(App_Suspending);
        }

        public class locationManager
        {
            public async static Task<Geoposition> GetPosition()
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                if (accessStatus != GeolocationAccessStatus.Allowed) throw new Exception();
                var geolocator = new Geolocator { DesiredAccuracyInMeters = 0 };
                var position = await geolocator.GetGeopositionAsync();
                return position;
            }
        }

        private void newEntry_Click(object sender, RoutedEventArgs e)
        {
            AddNewEntryPanel.Visibility = (AddNewEntryPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        private void cancelAdd_Click(object sender, RoutedEventArgs e)
        {
            AddNewEntryPanel.Visibility = (AddNewEntryPanel.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible);
        }

        private async void addEntry_Click(object sender, RoutedEventArgs e)
        {
            var position = await locationManager.GetPosition();
            latitudecord = "Latitude: " + position.Coordinate.Latitude.ToString("0.0000000000");
            longitudecord = "Longitude: " + position.Coordinate.Longitude.ToString("0.0000000000");
            currentTime = DateTime.Now.ToString("HH:mm", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            currentDate = DateTime.Now.ToString("M/d");
            entryList.Add(new Entry { entryId = listBoxID, entry = entryTextBox.Text, latitude = latitudecord, longitude = longitudecord, time = currentTime, date = currentDate });
            entryListFilter.Add(new Entry { entryId = listBoxID, entry = entryTextBox.Text, latitude = latitudecord, longitude = longitudecord, time = currentTime, date = currentDate });
        }

        private async void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            indexAt = ItemsView.SelectedIndex;
            entryListFilterCount = entryListFilter.Count;
            entryCount = entryList.Count;
            try
            {
                if (entryListFilterCount != entryCount)
                {
                    var selectedEntry = entryList[ItemsView.SelectedIndex];
                    entryList.RemoveAt(ItemsView.SelectedIndex);
                    if (entryListFilter.Contains(selectedEntry))
                    {
                        var pos = entryListFilter.IndexOf(selectedEntry);
                        entryListFilter.RemoveAt(pos);
                    }
                }
                else
                {
                    entryList.RemoveAt(ItemsView.SelectedIndex);
                    entryListFilter.RemoveAt(indexAt);
                }
            } catch
            {
                var dialog = new MessageDialog("In order to delete an entry: Click on an entry then press the delete button.");
                await dialog.ShowAsync();
            }
        }

        private async void playButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
                index = ItemsView.SelectedIndex;
                textToSpeech = entryList[index].entry;
                Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(textToSpeech);

                var mediaElement = new MediaElement();
                mediaElement.SetSource(stream, stream.ContentType);
                mediaElement.Play();
            } catch
            {
                var dialog = new MessageDialog("In order to play an entry: Click on an entry then press the play button.");
                await dialog.ShowAsync();
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Home.IsSelected)
            {
                titleTextBlock.Text = "Home";
                listBoxID = "Home";
                GetItems("Home", entryList);
                menu.IsPaneOpen = false;
            }
            else if (Work.IsSelected)
            {
                titleTextBlock.Text = "Work";
                listBoxID = "Work";
                GetItems("Work", entryList);
                menu.IsPaneOpen = false;
            }
            else if (Shop.IsSelected)
            {
                titleTextBlock.Text = "Shop";
                listBoxID = "Shop";
                GetItems("Shop", entryList);
                menu.IsPaneOpen = false;
            }
            else if (Leisure.IsSelected)
            {
                titleTextBlock.Text = "Leisure";
                listBoxID = "Leisure";
                GetItems("Leisure", entryList);
                menu.IsPaneOpen = false;
            }
            else if (Events.IsSelected)
            {
                titleTextBlock.Text = "Events";
                listBoxID = "Events";
                GetItems("Events", entryList);
                menu.IsPaneOpen = false;
            }
            else if (Other.IsSelected)
            {
                titleTextBlock.Text = "Other";
                listBoxID = "Other";
                GetItems("Other", entryList);
                menu.IsPaneOpen = false;
            }
            else if (DisplayMenu.IsSelected)
            {
                menu.IsPaneOpen = true;
            }
        }

        public static void GetItems(string category, ObservableCollection<Entry> entryList)
        {
            entryList.Clear();
            for (int i = 0; i < entryListFilter.Count; i++)
            {
                if (entryListFilter[i].entryId == category)
                {
                    entryList.Add(entryListFilter[i]);
                }
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await deserializeJsonAsync();
            Home.IsSelected = true;
        }

        private async Task writeJsonAsync()
        {
            var serializer = new DataContractJsonSerializer(typeof(List<Entry>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(JSONFILENAME,CreationCollisionOption.ReplaceExisting))
            {
                serializer.WriteObject(stream, entryListFilter);
            }
        }

        private async Task deserializeJsonAsync()
        {
            try
            {
                List<Entry> myEntryListFilter;
                var jsonSerializer = new DataContractJsonSerializer(typeof(List<Entry>));
                var myStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(JSONFILENAME);
                myEntryListFilter = (List<Entry>)jsonSerializer.ReadObject(myStream);
                entryListFilter = myEntryListFilter;
            } catch
            {
                Debug.WriteLine("File does not exist");
            }
        }

        async void App_Suspending(Object sender, Windows.ApplicationModel.ISuspendingEventArgs e)
        {
            await writeJsonAsync();
        }
    }
}