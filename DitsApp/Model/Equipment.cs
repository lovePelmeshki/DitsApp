﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Equipment
    {
        public Equipment()
        {
            Events = new HashSet<Event>();
            Maintenances = new HashSet<Maintenance>();
        }

        public string Id { get; set; }
        public int TypeId { get; set; }
        public int PointId { get; set; }
        public int Status { get; set; }
        public DateTime CheckupDate { get; set; }
        public DateTime NextCheckupDate { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public DateTime? InstallDate { get; set; }

        public virtual Point Point { get; set; }
        public virtual EquipmentType Type { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
