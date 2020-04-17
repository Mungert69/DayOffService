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


        public static List<WorkBase> getWorkDay(DateTime startCheck, DateTime endCheck, DayOff.Data.DayOffContext context)
        {
            List<IUserBase> userQuery = getActiveUsers(startCheck,endCheck,context);
            List<WorkDay> workQuery = new List<WorkDay>();
            WorkBase workBase;
            List<WorkBase> workBases = new List<WorkBase>();
            string userName;
            workQuery = context.WorkDays.Where(u =>  u.WorkDate >= startCheck   && u.WorkDate<=endCheck ).ToList();
            foreach (WorkDay workDay in workQuery) {
                userName=userQuery.Where(u => u.ID == workDay.UserID).Select(s => s.FirstName).FirstOrDefault();
                workBase = new WorkBase(workDay.WorkID, workDay.WorkType, workDay.Duration, workDay.WorkDate, workDay.UserID,userName);
                workBases.Add(workBase);
            }

            return workBases;
        }
      

            public static List<IUserBase> getActiveTLPs(DateTime startCheck, DateTime endCheck, DayOff.Data.DayOffContext context)
        {
            List<UserBase> userQuery = new List<UserBase>();
            userQuery = context.Users.Where(u => u.UserType== UserBase.UserTypes.TLP && ((startCheck >= u.StartDate && startCheck <= u.EndDate) || (endCheck >= u.StartDate && endCheck <= u.EndDate))).Select(s => new UserBase(s.ID, s.LastName, s.FirstName, s.StartDate, s.EndDate, (float)s.noHalfDaysOff / 2, (float)s.noHolidays / 2, s.UserType)).OrderByDescending(o => o.UserType).ToList();

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

        public static List<IUserBase> getAllUsers( DayOff.Data.DayOffContext context)
        {
            List<UserBase> userQuery = new List<UserBase>();
            userQuery = context.Users.Select(s => new UserBase(s.ID, s.LastName, s.FirstName, s.StartDate, s.EndDate, (float)s.noHalfDaysOff / 2, (float)s.noHolidays / 2, s.UserType)).OrderByDescending(o => o.UserType).ToList();

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
