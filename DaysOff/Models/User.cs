using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayOff.Models
{
    public class User
    {

       
            public int ID { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }

            public int noHolidays { get; set; }

            public int noHalfDaysOff { get; set; }

            public ICollection<Holiday> Holidays { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }

    }
}
