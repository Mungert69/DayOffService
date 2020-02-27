using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Allocations
    {
        public int AllocId { get; set; }
        public int? PayId { get; set; }
        public int? BookingId { get; set; }
        public decimal? AllocAmt { get; set; }
        public string AllocNotes { get; set; }
    }
}
