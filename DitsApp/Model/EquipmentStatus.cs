using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class EquipmentStatus
    {
        public int Id { get; set; }
        public int MaintainerId { get; set; }
        public int EquipmentId { get; set; }
        public int PointId { get; set; }
        public int Status { get; set; }
        public DateTime ChangeDate { get; set; }
        public DateTime CheckupDate { get; set; }
        public DateTime NextCheckupDate { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime NextMaintenanceDate { get; set; }
        public DateTime InstallDate { get; set; }
        public string StatusType { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Employee Maintainer { get; set; }
        public virtual Point Point { get; set; }
    }
}
