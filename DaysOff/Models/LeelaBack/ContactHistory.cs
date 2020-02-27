using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class ContactHistory
    {
        public int Chid { get; set; }
        public int? CtId { get; set; }
        public bool Confidential { get; set; }
        public DateTime? Chdate { get; set; }
        public string Chdetails { get; set; }
        public DateTime? ChfollowUp { get; set; }
        public DateTime? Chdone { get; set; }
    }
}
