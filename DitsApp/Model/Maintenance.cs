using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Maintenance
    {
        public int Id { get; set; }
        public int MainteinerId { get; set; }
        public string EquipmentId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public DateTime? DurationDate { get; set; }
        public string Comment { get; set; }

        public virtual Equipment Equipment { get; set; }
        public virtual Employee Mainteiner { get; set; }
    }
}
