using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class EquipmentType
    {
        public EquipmentType()
        {
            Equipment = new HashSet<Equipment>();
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int? InstallDuration { get; set; }
        public int? MaintenanceDuration { get; set; }
        public int ClassId { get; set; }

        public virtual EquipmentClass Class { get; set; }
        public virtual ICollection<Equipment> Equipment { get; set; }
    }
}
