using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class Location
    {
        public Location()
        {
            Equipment = new HashSet<Equipment>();
        }

        public int LocationId { get; set; }
        public int? StationId { get; set; }
        public string Point { get; set; }
        public string Desctiption { get; set; }

        public virtual Station Station { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
