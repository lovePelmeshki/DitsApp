using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class Maintenance
    {
        public int MaintenanceId { get; set; }
        public int? MaintainerId { get; set; }
        public string EquipmentId { get; set; }
        public DateTime? MaintenanceDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Comment { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Employee Maintainer { get; set; }
    }
}
