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
        private WorkTypes? amWorkType;
        private WorkTypes? pmWorkType;
        private int dishCount;
        private DateTime lastDate;
        private DateTime lastButOneDate;
        private DateTime lastButTwoDate;
        private DateTime lastButThreeDate;


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
        public WorkTypes? AmWorkType { get => amWorkType; set => amWorkType = value; }
        public WorkTypes? PmWorkType { get => pmWorkType; set => pmWorkType = value; }
        public int DishCount { get => dishCount; set => dishCount = value; }
        public DateTime LastDate { get => lastDate; set => lastDate = value; }
        public DateTime LastButOneDate{ get => lastButOneDate; set => lastButOneDate = value; }
        public DateTime LastButTwoDate { get => lastButTwoDate; set => lastButTwoDate = value; }
        public DateTime LastButThreeDate { get => lastButThreeDate; set => lastButThreeDate = value; }
    }
}
