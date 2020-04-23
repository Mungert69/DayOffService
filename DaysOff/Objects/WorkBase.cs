using System;
using System.Collections.Generic;

namespace DaysOff.Objects
{
    public enum WorkTypes
    {
        OHC=0, HC=1, MC=2, KA=3, DB=4, GR=5, OGR=6, IT=7, BY=8, PR=9, OJ=10, SH=11, SR=12, DBR=13,LC=14, RAP=15, IG=16, ID=17
    }

   

    public class WorkBase : EventBase
    {

        private List<string> workNames = new List<string>();
        private List<string> workExcelCols = new List<string>();
        private string userName;

        private void setWorkNames() {
            WorkNames.Add("Over house care");
            WorkExcelCols.Add("C");
            WorkNames.Add("House care");
            WorkExcelCols.Add("C");
            WorkNames.Add("Main cook");
            WorkExcelCols.Add("H");
            WorkNames.Add("Kitchen assistant");
            WorkExcelCols.Add("H");
            WorkNames.Add("Debop");
            WorkExcelCols.Add("D");
            WorkNames.Add("Grounds");
            WorkExcelCols.Add("E");
            WorkNames.Add("Over grounds");
            WorkExcelCols.Add("E");
            WorkNames.Add("Computers");
            WorkExcelCols.Add("F");
            WorkNames.Add("Backyard");
            WorkExcelCols.Add("F");
            WorkNames.Add("Promotions");
            WorkExcelCols.Add("F");
            WorkNames.Add("Own jobs");
            WorkExcelCols.Add("F");
            WorkNames.Add("Shopping");
            WorkExcelCols.Add("F");
            WorkNames.Add("Sewing Repair");
            WorkExcelCols.Add("F");
            WorkNames.Add("Debop Restoration");
            WorkExcelCols.Add("D");
            WorkNames.Add("Plant Care");
            WorkExcelCols.Add("F");
            WorkNames.Add("RAP Management");
            WorkExcelCols.Add("F");
            WorkNames.Add("Internal Group");
            WorkNames.Add("Internal Time Off");

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
        public WorkBase(int workID, WorkTypes? workType, Durations? duration, DateTime workDate, int userID,string userName) : base(workID)
        {
            WorkType = workType;
            Duration = duration;
            EventDate = workDate;
            UserID = userID;
            UserName = userName;
            EventType = (EventTypes)1;
            setWorkNames();
        }


        public string ExcelCol() {
            return workExcelCols[Convert.ToInt32(WorkType)];
            }
        public string DurationString() {
            if (Duration == 0) { return "Am"; }
            else  { return "Pm"; }
        }

        public WorkTypes? WorkType { get; set; }
        public List<string> WorkNames { get => workNames; set => workNames = value; }
        public List<string> WorkExcelCols { get => workExcelCols; set => workExcelCols = value; }
        public string UserName { get => userName; set => userName = value; }
    }
}
