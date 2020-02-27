using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class PriceTypes
    {
        public string Ptcode { get; set; }
        public string Ptdesc { get; set; }
        public bool Ptchild { get; set; }
        public bool Ptextra { get; set; }
        public decimal? GlobalPrice { get; set; }
    }
}
