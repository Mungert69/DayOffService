using DayOff.Data;
using DayOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public  class WeekDataCreator
    {

        public static EventBase getData(DateTime date, UserBase user, DayOffContext _context, int duration ){
            Holiday holData = _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date && h.Duration == (Durations)1).FirstOrDefault();
            if (date < user.StartDate || date > user.EndDate)
            {
                return new WorkBase(-2, 0, (Durations)duration, date, user.ID);

            }
            else
            {
                if (holData == null)
                {
                    WorkDay workData = _context.WorkDays.Where(w => w.UserID == user.ID && w.WorkDate == date && w.Duration == (Durations)duration).FirstOrDefault();
                    if (workData == null)
                    {
                        return new WorkBase(-1, 0, (Durations)duration, date, user.ID);
                    }
                    else
                    {
                        return new WorkBase(workData.WorkID, (WorkTypes)workData.WorkType, (Durations)workData.Duration, date, user.ID);
                    }
                }
                else
                {
                    return new HolidayBase(holData.HolidayID, (HolTypes)holData.HolType, (Durations)holData.Duration, holData.HolDate, user.ID);
                }
            }
        }
    }
}
