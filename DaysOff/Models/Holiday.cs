using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DayOff.Models
{


    public class Holiday
    {
        public enum HolTypes
        {
            D, H, X, HD
        }

        public enum Durations
        {
            AM, PM, DO
        }
   
        public int HolidayID { get; set; }
        public int UserID { get; set; }
       
        public HolTypes? HolType { get; set; }
        public Durations? Duration { get; set; }
        public DateTime HolDate { get; set; }

        public User User { get; set; }



    }

}
