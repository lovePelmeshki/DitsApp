using System;
using System.Collections.Generic;

#nullable disable

namespace DitsApp.Model
{
    public partial class Event
    {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public int StationId { get; set; }
        public int? LocationId { get; set; }
        public int? EquipmentId { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? RespondDate { get; set; }
        public int? RespoinderId { get; set; }
        public int? CreatorId { get; set; }
        public string Comment { get; set; }

        public virtual Station Station { get; set; }
        public virtual EventType Type { get; set; }
    }
}
