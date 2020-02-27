using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class EventPrices
    {
        public int EpriceId { get; set; }
        public int? EventId { get; set; }
        public string Ptcode { get; set; }
        public decimal? EpriceAmt { get; set; }
    }
}
