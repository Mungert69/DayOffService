using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class HolidayBase
    {

        public HolidayBase(int id)
        {
            HolidayID = id;
        }

        public HolidayBase(int holidayID, HolTypes? holType, Durations? duration, DateTime holDate,int userID) : this(holidayID)
        {
            HolType = holType;
            Duration = duration;
            HolDate = holDate;
            UserID = userID;
        }

        public enum HolTypes
        {
            H, D, X
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


    }
}
