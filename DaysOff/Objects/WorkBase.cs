using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public enum WorkTypes
    {
        OHC,HC,MK,K,DB,G,OG
    }


    public class WorkBase : EventBase
    {


        public WorkBase(int workID, WorkTypes? workType, Durations? duration, DateTime workDate, int userID) : base(workID)
        {
            WorkType = workType;
            Duration = duration;
            EventDate = workDate;
            UserID = userID;
            EventType = (EventTypes)1;
        }



        public WorkTypes? WorkType { get; set; }




    }
}
