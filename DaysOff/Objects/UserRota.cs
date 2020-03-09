using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class UserRota : IUserBase
    {
        private bool isAmOff;
        private bool isPmOff;

        public bool IsAmOff { get => isAmOff; set => isAmOff = value; }
        public bool IsPmOff { get => isPmOff; set => isPmOff = value; }
        public float DaysAllowedPerWeek { get; set; }
        public DateTime EndDate { get; set; }
        public string FirstName { get; set; }
        public float HolidaysTaken { get; set; }
        public int ID { get; set; }
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public float TotalHolidays { get; set; }
        public UserBase.UserTypes? UserType { get; set; }

        
    }
}
