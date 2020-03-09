using DayOff.Models;
using DaysOff.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Utils
{
    public class DataBaseHelper
    {

       
        public static List<IUserBase> getActiveUsers(DateTime startCheck, DateTime endCheck, DayOff.Data.DayOffContext context)
        {
            List<UserBase> userQuery = new List<UserBase>();
            userQuery = context.Users.Where(u => (startCheck >= u.StartDate && startCheck <= u.EndDate) || (endCheck >= u.StartDate && endCheck <= u.EndDate)).Select(s => new UserBase(s.ID, s.LastName, s.FirstName, s.StartDate, s.EndDate, (float)s.noHalfDaysOff / 2, (float)s.noHolidays / 2, s.UserType)).OrderByDescending(o => o.UserType).ToList();

            List<Holiday> holidays;
            List<IUserBase> usersBase = new List<IUserBase>();
            foreach (UserBase userBase in userQuery)
            {
                holidays = context.Holidays.Where(h => h.UserID == userBase.ID && h.HolDate >= userBase.StartDate && h.HolDate <= userBase.EndDate && h.HolType == HolTypes.H).ToList();
                userBase.HolidaysTaken = (float)holidays.Count() / 2;
                usersBase.Add(userBase);
            }
            return usersBase;
        }

    }
}
