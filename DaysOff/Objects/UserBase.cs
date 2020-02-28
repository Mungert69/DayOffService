using System;

namespace DaysOff.Objects
{
    public class UserBase
    {

        private float totalHolidays;
        private float holidaysTaken;
        private float daysAllowedPerWeek;

        public UserBase(int iD, string lastName, string firstName, DateTime startDate, DateTime endDate)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            StartDate = startDate;
            EndDate = endDate;

        }

        public UserBase(int iD, string lastName, string firstName, DateTime startDate, DateTime endDate, float daysAllowedPerWeek, float totalHolidays)
        {
            ID = iD;
            LastName = lastName;
            FirstName = firstName;
            StartDate = startDate;
            EndDate = endDate;
            DaysAllowedPerWeek = daysAllowedPerWeek;
            TotalHolidays = totalHolidays;

        }



        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public float TotalHolidays { get => totalHolidays; set => totalHolidays = value; }
        public float HolidaysTaken { get => holidaysTaken; set => holidaysTaken = value; }
        public float DaysAllowedPerWeek { get => daysAllowedPerWeek; set => daysAllowedPerWeek = value; }
    }
}
