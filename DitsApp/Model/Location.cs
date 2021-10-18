using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Location
    {
        public Location()
        {
            Events = new HashSet<Event>();
            Points = new HashSet<Point>();
        }

        public int LocationId { get; set; }
        public int StationId { get; set; }
        public string LocationName { get; set; }

        public virtual Station Station { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
