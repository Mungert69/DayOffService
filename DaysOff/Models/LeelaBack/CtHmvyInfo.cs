using System;
using System.Collections.Generic;

namespace DaysOff.Models.LeelaBack
{
    public partial class CtHmvyInfo
    {
        public int CtHmvyId { get; set; }
        public DateTime? LastHiv { get; set; }
        public DateTime? NextHiv { get; set; }
        public DateTime? Y1start { get; set; }
        public DateTime? Y1complete { get; set; }
        public DateTime? Y2start { get; set; }
        public DateTime? Y2complete { get; set; }
    }
}
