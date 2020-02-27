using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Payments
    {
        public int PayId { get; set; }
        public int? CtId { get; set; }
        public decimal? PayAmt { get; set; }
        public string PmethodCode { get; set; }
        public DateTime? PayDate { get; set; }
        public string Payer { get; set; }
        public int? LeelaUserCt { get; set; }
        public string PayNotes { get; set; }
        public DateTime? PayClosed { get; set; }
    }
}
