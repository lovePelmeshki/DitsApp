using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class Equipment
    {
        public Equipment()
        {
            Events = new HashSet<Event>();
            Maintenances = new HashSet<Maintenance>();
        }

        public string EquipmentId { get; set; }
        public int? TypeId { get; set; }
        public int? LocationId { get; set; }
        public int? MaintenanceId { get; set; }
        public int? InstallId { get; set; }

        public virtual Location Location { get; set; }
        public virtual EquipmentType Type { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
