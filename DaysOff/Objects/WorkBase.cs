using System;
using System.Collections.Generic;

namespace DaysOff.Objects
{
    public enum WorkTypes
    {
        OHC=0, HC=1, MC=2, KA=3, DB=4, GR=5, OGR=6, IT=7, BY=8, PR=9, OJ=10, SH=11, SR=12, LC=13,PL=14, RAP=15
    }

   

    public class WorkBase : EventBase
    {

        private List<string> workNames = new List<string>();

        private void setWorkNames() {
            WorkNames.Add("Over house care");
            WorkNames.Add("House care");
            WorkNames.Add("Main cook");
            WorkNames.Add("Kitchen assistant");
            WorkNames.Add("Debop");
            WorkNames.Add("Grounds");
            WorkNames.Add("Over grounds");
            WorkNames.Add("Computers");
            WorkNames.Add("Backyard");
            WorkNames.Add("Promotions");
            WorkNames.Add("Own jobs");
            WorkNames.Add("Shopping");
            WorkNames.Add("Sewing Repair");
            WorkNames.Add("Linen Cuboard");
            WorkNames.Add("Plant Care");
            WorkNames.Add("RAP Management");

        }

        public WorkBase() {
            setWorkNames();
        }
        public WorkBase(int workID, WorkTypes? workType, Durations? duration, DateTime workDate, int userID) : base(workID)
        {
            WorkType = workType;
            Duration = duration;
            EventDate = workDate;
            UserID = userID;
            EventType = (EventTypes)1;
            setWorkNames();
        }



        public WorkTypes? WorkType { get; set; }
        public List<string> WorkNames { get => workNames; set => workNames = value; }
    }
}
