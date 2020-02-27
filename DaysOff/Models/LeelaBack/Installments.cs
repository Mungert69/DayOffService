using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Installments
    {
        public int InstId { get; set; }
        public int? BookingId { get; set; }
        public byte? InstNo { get; set; }
        public DateTime? InstExpected { get; set; }
        public decimal? InstAmount { get; set; }
        public string InstNotes { get; set; }
    }
}
