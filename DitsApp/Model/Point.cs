using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Point
    {
        public Point()
        {
            EquipmentStatuses = new HashSet<EquipmentStatus>();
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public string PointName { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<EquipmentStatus> EquipmentStatuses { get; set; }
    }
}
