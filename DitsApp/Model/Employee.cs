using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Employee
    {
        public Employee()
        {
            EquipmentStatuses = new HashSet<EquipmentStatus>();
            Maintenances = new HashSet<Maintenance>();
        }

        public int Id { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public int DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<EquipmentStatus> EquipmentStatuses { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
