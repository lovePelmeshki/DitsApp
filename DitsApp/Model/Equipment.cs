using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Equipment
    {
        public Equipment()
        {
            EquipmentStatuses = new HashSet<EquipmentStatus>();
            Events = new HashSet<Event>();
            Maintenances = new HashSet<Maintenance>();
        }

        public int Id { get; set; }
        public int TypeId { get; set; }
        public string SerialNumber { get; set; }

        public virtual EquipmentType Type { get; set; }
        public virtual ICollection<EquipmentStatus> EquipmentStatuses { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
