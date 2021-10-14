using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Point
    {
        public int PointId { get; set; }
        public int? LocationId { get; set; }
        public string PointName { get; set; }

        public virtual Location Location { get; set; }
    }
}
