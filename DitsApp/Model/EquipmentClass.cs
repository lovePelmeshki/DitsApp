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

        public int Id { get; set; }
        public string ClassName { get; set; }

        public virtual ICollection<EquipmentType> EquipmentTypes { get; set; }
    }
}
