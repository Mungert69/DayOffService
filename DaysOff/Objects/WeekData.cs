using DaysOff.Models.LeelaBack;
using System;
using System.Collections.Generic;

namespace DaysOff.Objects
{
    public class WeekData
    {
        private List<DateTime> headerDates;
        private List<UserDataRow> userDataRows;
        private EventData eventData;
        private List<DayWorkObj> dayWorkObjs;


        public List<DateTime> HeaderDates { get => headerDates; set => headerDates = value; }
        public List<UserDataRow> UserDataRows { get => userDataRows; set => userDataRows = value; }
        public EventData EventData { get => eventData; set => eventData = value; }
        public List<DayWorkObj> DayWorkObjs { get => dayWorkObjs; set => dayWorkObjs = value; }
    }
}
