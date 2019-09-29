using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class UserBase
    {
        public UserBase(int iD, string lastName, string firstName, DateTime startDate, DateTime endDate)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            StartDate = startDate;
            EndDate = endDate;
        }

        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
