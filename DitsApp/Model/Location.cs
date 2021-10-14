using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Location
    {
        public Location()
        {
            Equipment = new HashSet<Equipment>();
            Points = new HashSet<Point>();
        }

        public int LocationId { get; set; }
        public int? StationId { get; set; }
        public string LocationName { get; set; }

        public virtual Station Station { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
        public virtual ICollection<Point> Points { get; set; }
    }
}
