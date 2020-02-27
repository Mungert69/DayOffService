using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Events
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public int? LeaderCt { get; set; }
        public int? CoLeaderCt { get; set; }
        public DateTime? EventStart { get; set; }
        public DateTime? EventEnd { get; set; }
        public string EcatRef { get; set; }
        public bool UseExtraA { get; set; }
        public string ExtraAcaption { get; set; }
        public bool UseExtraB { get; set; }
        public string ExtraBcaption { get; set; }
        public string Qbcode { get; set; }
        public DateTime? EventCancelled { get; set; }
        public string BookingInstructions { get; set; }
        public string AddChildLetter { get; set; }
    }
}
