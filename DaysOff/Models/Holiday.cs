using DaysOff.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace DayOff.Models
{


    public class Holiday
    {

        [Key] public int HolidayID { get; set; }
        public int UserID { get; set; }

        public HolTypes? HolType { get; set; }
        public Durations? Duration { get; set; }
        public DateTime HolDate { get; set; }

        public User User { get; set; }

    }

}
