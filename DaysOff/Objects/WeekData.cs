using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class WeekData
    {
        private List<DateTime> headerDates ;       
        private List<UserDataRow> userDataRows ;

        public List<DateTime> HeaderDates { get => headerDates; set => headerDates = value; }
        public List<UserDataRow> UserDataRows { get => userDataRows; set => userDataRows = value; }
    }
}
