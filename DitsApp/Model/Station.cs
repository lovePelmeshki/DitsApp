using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Station
    {
        public Station()
        {
            Locations = new HashSet<Location>();
        }

        public int StationId { get; set; }
        public string StationName { get; set; }
        public int Line { get; set; }

        public virtual ICollection<Location> Locations { get; set; }
    }
}
