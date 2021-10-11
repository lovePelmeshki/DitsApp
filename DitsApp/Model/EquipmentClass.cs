using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class EquipmentClass
    {
        public EquipmentClass()
        {
            EquipmentTypes = new HashSet<EquipmentType>();
        }

        public int EquipmentClassId { get; set; }
        public string EquipmentClassName { get; set; }

        public virtual ICollection<EquipmentType> EquipmentTypes { get; set; }
    }
}
