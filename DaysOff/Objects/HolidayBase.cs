﻿using System;

namespace DaysOff.Objects
{
    public enum HolTypes
    {
        H, D, X
    }


    public class HolidayBase : EventBase
    {


        public HolidayBase(int holidayID, HolTypes? holType, Durations? duration, DateTime holDate, int userID) : base(holidayID)
        {
            HolType = holType;
            Duration = duration;
            EventDate = holDate;
            UserID = userID;
            EventType = 0;
        }



        public HolTypes? HolType { get; set; }




    }
}
