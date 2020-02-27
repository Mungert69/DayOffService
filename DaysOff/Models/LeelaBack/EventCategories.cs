using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class EventCategories
    {
        public string EcatRef { get; set; }
        public string EcatDesc { get; set; }
        public bool EcatGuests { get; set; }
        public bool Tsequence { get; set; }
    }
}
