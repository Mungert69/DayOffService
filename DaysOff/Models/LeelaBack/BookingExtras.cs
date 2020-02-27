using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class BookingExtras
    {
        public int Beid { get; set; }
        public int? BookingId { get; set; }
        public decimal? Beprice { get; set; }
        public byte? Beqty { get; set; }
        public string Benotes { get; set; }
        public int? EpriceId { get; set; }
    }
}
