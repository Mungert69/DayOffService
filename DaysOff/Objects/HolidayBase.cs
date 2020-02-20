using System;
using System.Collections.Generic;

namespace DaysOff.Objects
{
    public enum HolTypes
    {
        D=1, H=0, X=2, G=3
    }

    

    public class HolidayBase : EventBase
    {

        private List<string> holNames=new List<string>();
        private void setHolNames() {
            HolNames.Add("Holiday");
            HolNames.Add("Time Off");
            HolNames.Add("Away");
            HolNames.Add("Group");

        }

        public HolidayBase() {
            setHolNames();
        }
        public HolidayBase(int holidayID, HolTypes? holType, Durations? duration, DateTime holDate, int userID) : base(holidayID)
        {
            HolType = holType;
            Duration = duration;
            EventDate = holDate;
            UserID = userID;
            EventType = 0;
            setHolNames();
          
        }



        public HolTypes? HolType { get; set; }
        public List<string> HolNames { get => holNames; set => holNames = value; }
    }
}
