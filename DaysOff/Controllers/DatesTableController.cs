using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DayOff.Data;
using DayOff.Models;
using DaysOff.ExtensionMethods;
using DaysOff.Objects;
using DaysOff.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DaysOff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatesTableController : ControllerBase
    {
        private readonly DayOffContext _context;

        public DatesTableController(DayOffContext context)
        {
            _context = context;
        }

        private List<string> getTypes()
        {
            List<string> types = new List<string>();
            return Enum.GetValues(typeof(Holiday.HolTypes))
     .Cast<Holiday.HolTypes>()
     .Select(v => v.ToString())
     .ToList();

        }

        private List<string> getDurations()
        {
            List<string> durations = new List<string>();

            return Enum.GetValues(typeof(Holiday.Durations))
    .Cast<Holiday.Durations>()
    .Select(v => v.ToString())
    .ToList();
        }
        private List<UserBase> getActiveUsers()
        {
            List<UserBase> users = new List<UserBase>();
            users = _context.Users.Where(u => u.StartDate < DateTime.Now && u.EndDate > DateTime.Now).Select(s => new UserBase(s.ID, s.LastName, s.FirstName, s.StartDate, s.EndDate)).ToList();

            return users;
        }

        private int convertToHalfDays(int duration,int type)
        {
            switch (type) {
                case 1:
                    return 0;
                case 2:
                    return 0;
                case 3:
                    return 1;
            }

            switch (duration)
            {
                case 0:
                    return 1;
                case 1:
                    return 1;
                case 2:
                    return 2;            
            }
            return 0;
        }
        private bool countDaysOk( DateTime holDate, int userId) {
            DateTime startDate = holDate.StartOfWeek(DayOfWeek.Monday);
            DateTime endDate = startDate.AddDays(6) ;
            List<Holiday> holidays = _context.Holidays.Where(h => h.UserID == userId && h.HolDate >= startDate && h.HolDate <=endDate).ToList();
            int halfDays = 0;
            foreach (Holiday hol in holidays)
            {
                halfDays += convertToHalfDays((int)hol.Duration,(int)hol.HolType); 
            }
            int userHalfDays = _context.Users.Where(h => h.ID == userId).Select(s => s.noHalfDaysOff).FirstOrDefault();
            if (halfDays < userHalfDays) { return false; }
            else { return true; }
           
        }

        private bool countDaysOk(DateTime holDate, int userId,int duration, int type)
        {
            DateTime startDate = holDate.StartOfWeek(DayOfWeek.Monday);
            DateTime endDate = startDate.AddDays(6);
            List<Holiday> holidays = _context.Holidays.Where(h => h.UserID == userId && h.HolDate >= startDate && h.HolDate <= endDate).ToList();
            int halfDays = 0;
            foreach (Holiday hol in holidays)
            {
                halfDays += convertToHalfDays((int)hol.Duration, (int)hol.HolType);
            }
            halfDays += convertToHalfDays(duration, type);
            int userHalfDays = _context.Users.Where(h => h.ID == userId).Select(s => s.noHalfDaysOff).FirstOrDefault();
            if (halfDays <= userHalfDays) { return false; }
            else { return true; }

        }
        private bool countHolidaysOk(DateTime holDate, int userId, int duration, int type)
        {
            throw new NotImplementedException();
        }

        // GET api/DatesTable/WeekData
        [HttpGet("WeekData/{fromStr}/{toStr}")]
        public ActionResult<WeekData> WeekData([FromRoute] string fromStr, [FromRoute] string toStr)
        {
            DateTime from;
            DateTime to;
            try
            {
                from = DateTime.Parse(fromStr);
                to = DateTime.Parse(toStr);
            }
            catch { return null; }


            List<DateTime> headerDates = new List<DateTime>();
            List<HolidayBase> userRow = new List<HolidayBase>();
            List<UserBase> users = getActiveUsers();
            List<UserDataRow> userDataRows = new List<UserDataRow>();
            WeekData weekData = new WeekData();

            headerDates.Add(from);
            for (int i = 1; i < 6; i++)
            {
                headerDates.Add(from.AddDays(i));
            }
            headerDates.Add(to);

            foreach (UserBase user in users)
            {
                userRow = new List<HolidayBase>();
                foreach (DateTime date in headerDates)
                {
                    var holData = _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date).FirstOrDefault();
                    if (holData == null)
                    {
                        userRow.Add(new HolidayBase(-1, (HolidayBase.HolTypes)2, (HolidayBase.Durations)2, date, user.ID));
                    }
                    else
                    {
                        userRow.Add(new HolidayBase(holData.HolidayID, (HolidayBase.HolTypes)holData.HolType, (HolidayBase.Durations)holData.Duration, holData.HolDate, user.ID));
                    }
                }
                UserDataRow userDataRow = new UserDataRow();
                userDataRow.User = user;
                userDataRow.UserRow = userRow;
                userDataRows.Add(userDataRow);
            }

            weekData.HeaderDates = headerDates;
            weekData.UserDataRows = userDataRows;
            return weekData;
        }

        // GET api/DatesTable/GetTypes
        [HttpGet("GetDurations")]
        public ActionResult<IEnumerable<string>> GetDurations()
        {
            return getDurations();
        }

        // GET api/DatesTable/GetUsers
        [HttpGet("GetUsers")]
        public ActionResult<IEnumerable<UserBase>> GetUsers()
        {
            return getActiveUsers();
        }


        // GET api/DatesTable/GetTypes
        [HttpGet("GetTypes")]
        public ActionResult<IEnumerable<string>> GetTypes()
        {
            return getTypes();
        }

        // GET api/DatesTable/ActiveUsers
        [HttpGet("ActiveUsers")]
        public ActionResult<IEnumerable<UserBase>> ActiveUsers()
        {
            return getActiveUsers();
        }

        // GET api/DatesTable/AllUsers
        [HttpGet("AllUsers")]
        public ActionResult<IEnumerable<User>> AllUsers()
        {
            List<User> users = new List<User>();
            users = _context.Users.ToList();

            return users;
        }

        // GET api/DatesTable/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/DatesTable
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // GET api/DatesTable/DeleteHoliday/holidayID 
        [HttpGet("DeleteHoliday/{holidayId}")]
        public IActionResult DeleteHoliday([FromRoute] int holidayId)
        {
            string result = "Delete failed.";

            try
            {
                DayOff.Models.Holiday holiday = _context.Holidays.Find(holidayId);


                _context.Remove(holiday);
                _context.SaveChanges();
            }

            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }
            result = "Deleted ok.";
            return Ok(JsonUtils.ConvertJsonStr(result));
            ;
        }


        // GET api/DatesTable/UpdateHoliday/userId/type/duration/date 
        [HttpGet("CreateHoliday/{userId}/{type}/{duration}/{dateStr}")]
        public IActionResult CreateHoliday([FromRoute] int userId, [FromRoute] int type, [FromRoute] int duration, [FromRoute] string dateStr)
        {
            string result = "Create failed.";
            DateTime holDate;
           
            try
            {
                holDate = DateTime.Parse(dateStr);
                if (countDaysOk(holDate, userId,duration,type))
                {
                    result = "To many days off selected.";
                    return Ok(JsonUtils.ConvertJsonStr(result));
                }
               /* if (countHolidaysOk(holDate, userId, duration, type))
                {
                    result = "To many holidays selected.";
                    return Ok(JsonUtils.ConvertJsonStr(result));
                }*/

                DayOff.Models.Holiday holiday = new Holiday();
                holiday.HolType = (Holiday.HolTypes)type;
                holiday.Duration = (Holiday.Durations)duration;
                holiday.UserID = userId;
                holiday.HolDate = holDate;


                _context.Add(holiday);
                _context.SaveChanges();

            }
            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }


            result = "Created ok.";

            return Ok(JsonUtils.ConvertJsonStr(result));
        }

        


        // GET api/DatesTable/UpdateHoliday/id/type/duration 
        [HttpGet("UpdateHoliday/{id}/{type}/{duration}")]
        public IActionResult UpdateHoliday([FromRoute] int id, [FromRoute] int type, [FromRoute] int duration)
        {
            string result = "Update failed.";
            if (id == -1)
            {
                return Ok(JsonUtils.ConvertJsonStr("Update failed. HolidayID=-1"));
       
            }
            try
            {
                DayOff.Models.Holiday holiday = _context.Holidays.Find(id);
                holiday.HolType = (Holiday.HolTypes)type;
                holiday.Duration = (Holiday.Durations)duration;
                if (countDaysOk(holiday.HolDate, holiday.UserID))
                {
                    result = "To many days off selected.";
                    return Ok(JsonUtils.ConvertJsonStr(result));
                }
                _context.Update(holiday);
                _context.SaveChanges();
            }
            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }

            result = "Updated Ok.";
            return Ok(JsonUtils.ConvertJsonStr(result));
        }
        // PUT api/DatesTable/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/DatesTable/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }


}
