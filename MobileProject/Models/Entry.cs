using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProject.Models
{
    public class Entry
    {
        public string entryId { get; set; }
        public string entry { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string time { get; set; }
        public string date { get; set; }
    }
}
