using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class Employee
    {
        public Employee()
        {
            EventCreators = new HashSet<Event>();
            EventRespoinders = new HashSet<Event>();
            Maintenances = new HashSet<Maintenance>();
        }

        public int EmployeeId { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public int? DepartmentId { get; set; }

        public virtual Department Department { get; set; }
        public virtual ICollection<Event> EventCreators { get; set; }
        public virtual ICollection<Event> EventRespoinders { get; set; }
        public virtual ICollection<Maintenance> Maintenances { get; set; }
    }
}
