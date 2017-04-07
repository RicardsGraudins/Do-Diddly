# Do-Diddly
This repository contains code and information for my third-year undergraduate project for the module **Mobile Application Developement 2.**
The module is taught to undergraduate students at [GMIT](http://www.gmit.ie/) in the Department of Computer Science and Applied Physics.
The lecturer is Damien Costello.

#### Project Guidelines
> You are required to create a Universal Windows Project (UWP) that will demonstrate the use of Isolated Storage and at least one other
sensor or service available on the devices.  These include:
>* Accelerometer or Gyroscope
>* GPS (Location Based Services)
>* Sound
>* Network Services (connecting to server for data updates etc) 
>* Camera
>* Multi Touch Gesture Management

>The UWP application should be well designed with a clear purpose and an answer to the question “why will
the user open this app for a second time?”

#### Project Overview
I created an application that stores user's entries locally and provides time, date and GPS location of the entry. The purpose of the application
is to behave like a journal with a user-friendly UI that is easy to use. It is primarily targeted at people with short term memory and the 
app allows them to quickly make an entry and save it to be viewed later. Memory loss is something that everyone experiences at times however
an entry in the application can make the user recall what he/she was thinking and if the entry, date and time is not enough the user can take
the GPS cordinates to pinpoint the exact location of the entry.

#### Application Functionality
There are 6 categories where the user can store an entry, the default is "Home". To add a new entry click on the "+" button, enter your
data and click "OK" to add the entry or "Cancel" to cancel. Time, date and GPS data is automatically added whenever a new entry is added.
The user can delete using the delete button or playback the entry using Microsoft's Speech Synthesizer by clicking on the play button. An entry
must be selected by the user in order to use the delete or playback features. If the application gets suspended all the entries are written
to a json file on the local storage and are loaded into the application when it starts.

#### Quick installation guide:
The application is fully written using Visual Studio 2015 so there are 2 ways to run it:
1. Locate MobileProject_1.1.4.0_x86_x64_arm_bundle.appxupload inside MobileProject/AppPackages and double click to install. If that fails
refer to https://www.maketecheasier.com/install-appx-files-windows-10/to get it working in under 5 minutes.
2. If you have Visual Studio installed, select file open project/solution and locate MobileProject.sln to open the application and then
run it.
