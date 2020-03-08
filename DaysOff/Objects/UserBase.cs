using System;

namespace DaysOff.Objects
{
    public class UserBase
    {
        public enum UserTypes
        {
            RAP,TLP,Core,Director
        }

       
        public UserBase(int iD, string lastName, string firstName, DateTime startDate, DateTime endDate)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            StartDate = startDate;
            EndDate = endDate;

        }

        public UserBase(int iD, string lastName, string firstName, DateTime startDate, DateTime endDate, float daysAllowedPerWeek, float totalHolidays, UserTypes? userType)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            StartDate = startDate;
            EndDate = endDate;
            DaysAllowedPerWeek = daysAllowedPerWeek;
            TotalHolidays = totalHolidays;
            UserType = userType;

        }



        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float TotalHolidays { get ; set; }
        public float HolidaysTaken { get; set ; }
        public float DaysAllowedPerWeek { get ; set ; }

        public UserTypes? UserType { get; set; }
    }
}
