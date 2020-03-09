using System;

namespace DaysOff.Objects
{
    public interface IUserBase
    {
        float DaysAllowedPerWeek { get; set; }
        DateTime EndDate { get; set; }
        string FirstName { get; set; }
        float HolidaysTaken { get; set; }
        int ID { get; set; }
        string LastName { get; set; }
        DateTime StartDate { get; set; }
        float TotalHolidays { get; set; }
        UserBase.UserTypes? UserType { get; set; }
    }
}