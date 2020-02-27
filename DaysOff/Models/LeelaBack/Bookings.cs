using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class Bookings
    {
        public int BookingId { get; set; }
        public int? CtId { get; set; }
        public int? EventId { get; set; }
        public int? RoleId { get; set; }
        public int? AccomId { get; set; }
        public bool ExtraNight { get; set; }
        public bool ExtraDinner { get; set; }
        public bool CustomA { get; set; }
        public bool CustomB { get; set; }
        public DateTime? BkDate { get; set; }
        public decimal? BkPrice { get; set; }
        public int? MainBooking { get; set; }
        public int? BookerCt { get; set; }
        public DateTime? ConfSent { get; set; }
        public bool CheckedIn { get; set; }
        public DateTime? BkCancelled { get; set; }
        public string BkNotes { get; set; }
        public DateTime? BkClosed { get; set; }
        public int? EpriceId { get; set; }
    }
}
