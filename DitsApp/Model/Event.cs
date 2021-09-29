using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp
{
    public partial class Event
    {
        public int EventId { get; set; }
        public string EquipmentId { get; set; }
        public int? EventTypeId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? RespondDate { get; set; }
        public int? RespoinderId { get; set; }
        public int? CreatorId { get; set; }
        public string Comment { get; set; }

        public virtual Employee Creator { get; set; }
        public virtual Equipment Equipment { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual Employee Respoinder { get; set; }
    }
}
