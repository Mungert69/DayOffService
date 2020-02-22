﻿using DayOff.Data;
using DayOff.Models;
using DaysOff.ExtensionMethods;
using DaysOff.Objects;
using DaysOff.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private List<SelectItem> getHolTypes()
        {
            List<SelectItem> holidays = new List<SelectItem>();
            SelectItem selectItem;
            HolidayBase holidayBase = new HolidayBase();
            List<HolTypes> holTypes = Enum.GetValues(typeof(HolTypes)).Cast<HolTypes>().ToList();
            foreach (HolTypes holType in holTypes)
            {
                selectItem = new SelectItem();
                selectItem.Label = holidayBase.HolNames[Convert.ToInt32(holType)];
                selectItem.Value = holType.ToString();
                selectItem.Id = Convert.ToInt32(holType);
                holidays.Add(selectItem);
            }
            return holidays;
           

        }

        private List<SelectItem> getWorkTypes()
        {
            List<SelectItem> types = new List<SelectItem>();
            SelectItem selectItem;
            WorkBase workBase = new WorkBase();
            List<WorkTypes> workTypes= Enum.GetValues(typeof(WorkTypes)).Cast<WorkTypes>().ToList();
            foreach (WorkTypes workType in workTypes) {
                selectItem = new SelectItem();
                selectItem.Label = workBase.WorkNames[Convert.ToInt32(workType)];
                selectItem.Value = workType.ToString();
                selectItem.Id = Convert.ToInt32(workType);

                types.Add(selectItem);
            }
            return types;
        }

        private List<string> getDurations()
        {
            List<string> durations = new List<string>();

            return Enum.GetValues(typeof(Durations))
    .Cast<Durations>()
    .Select(v => v.ToString())
    .ToList();
        }
        private List<UserBase> getActiveUsers(DateTime dateCheck)
        {
            List<UserBase> users = new List<UserBase>();
            users = _context.Users.Where(u => u.StartDate < DateTime.Now && u.EndDate > dateCheck).Select(s => new UserBase(s.ID, s.LastName, s.FirstName, s.StartDate, s.EndDate)).ToList();

            return users;
        }

        private int convertToHalfDays(int duration, int type)
        {
            switch (type)
            {
                case 0:
                    return 0;
                case 2:
                    return 0;
                case 3:
                    return 0;
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


        private bool staffCountOk(DateTime holDate, int duration, int type, bool isAm)
        {
            List<Holiday> holidays = _context.Holidays.Where(h => h.HolDate == holDate).ToList();
            int amCount = 0;
            int pmCount = 0;
            foreach (Holiday hol in holidays)
            {
                switch ((int)hol.HolType)
                {
                    case 3:
                        amCount++;
                        pmCount++;
                        continue;
                }
                switch ((int)hol.Duration)
                {
                    case 0:
                        amCount++;
                        break;
                    case 1:
                        pmCount++;
                        break;
                    case 2:
                        amCount++;
                        pmCount++;
                        break;

                }
            }
            if (type == 3)
            {
                amCount++;
                pmCount++;
            }
            else
            {
                switch (duration)
                {
                    case 0:
                        amCount++;
                        break;
                    case 1:
                        pmCount++;
                        break;
                    case 2:
                        amCount++;
                        pmCount++;
                        break;

                }
            }

            int staffTotal = getActiveUsers(holDate).Count();
            if (isAm)
            {
                if (amCount <= (staffTotal - 2)) { return false; }
            }
            else
            {
                if (pmCount <= (staffTotal - 2)) { return false; }
            }
            return true;

        }
        private bool countHolidaysOk(DateTime holDate, int userId, int duration, int type)
        {
            User user = _context.Users.Where(u => u.ID == userId).FirstOrDefault();
            DateTime startDate = user.StartDate;
            DateTime endDate = user.EndDate;
            List<Holiday> holidays = _context.Holidays.Where(h => h.UserID == userId && h.HolDate >= startDate && h.HolDate <= endDate && h.HolType == HolTypes.H).ToList();
            int halfDays = holidays.Count()+1;
            
          
             if (halfDays <= user.noHolidays) { return false; }
            else { return true; }

        }

        private bool countDaysOk(DateTime holDate, int userId, int duration, int type)
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
            List<EventBase> userRow = new List<EventBase>();
            List<UserBase> users = getActiveUsers(DateTime.Now);
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
                userRow = new List<EventBase>();
                foreach (DateTime date in headerDates)
                {
                    Holiday holData = _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date && h.Duration == 0).FirstOrDefault();
                    if (holData == null)
                    {
                        WorkDay workData = _context.WorkDays.Where(w => w.UserID == user.ID && w.WorkDate == date && w.Duration == 0).FirstOrDefault();
                        if (workData == null)
                        {
                            userRow.Add(new WorkBase(-1, 0, 0, date, user.ID));
                        }
                        else
                        {
                            userRow.Add(new WorkBase(workData.WorkID, (WorkTypes)workData.WorkType, (Durations)workData.Duration, date, user.ID));
                        }
                    }
                    else
                    {
                        userRow.Add(new HolidayBase(holData.HolidayID, (HolTypes)holData.HolType, (Durations)holData.Duration, holData.HolDate, user.ID));
                    }
                }
                UserDataRow userDataRow = new UserDataRow();
                userDataRow.User = user;
                userDataRow.UserRow = userRow;
                userDataRows.Add(userDataRow);

                userRow = new List<EventBase>();
                foreach (DateTime date in headerDates)
                {
                    Holiday holData = _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date && h.Duration == (Durations)1).FirstOrDefault();
                    if (holData == null)
                    {
                        WorkDay workData = _context.WorkDays.Where(w => w.UserID == user.ID && w.WorkDate == date && w.Duration == (Durations)1).FirstOrDefault();
                        if (workData == null)
                        {
                            userRow.Add(new WorkBase(-1, 0, (Durations)1, date, user.ID));
                        }
                        else
                        {
                            userRow.Add(new WorkBase(workData.WorkID, (WorkTypes)workData.WorkType, (Durations)workData.Duration, date, user.ID));
                        }
                    }
                    else
                    {
                        userRow.Add(new HolidayBase(holData.HolidayID, (HolTypes)holData.HolType, (Durations)holData.Duration, holData.HolDate, user.ID));
                    }
                }
                userDataRow = new UserDataRow();
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
            return getActiveUsers(DateTime.Now);
        }


        // GET api/DatesTable/GetHolTypes
        [HttpGet("GetHolTypes")]
        public ActionResult<IEnumerable<SelectItem>> GetHolTypes()
        {
            return getHolTypes();
        }

        // GET api/DatesTable/GetWorkTypes
        [HttpGet("GetWorkTypes")]
        public ActionResult<IEnumerable<SelectItem>> GetWorkTypes()
        {
            return getWorkTypes();
        }

        // GET api/DatesTable/ActiveUsers
        [HttpGet("ActiveUsers")]
        public ActionResult<IEnumerable<UserBase>> ActiveUsers()
        {
            return getActiveUsers(DateTime.Now);
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

        private void deleteIntEvent(int eventId, int eventType)
        {
            if (eventType == 0)
            {
                DayOff.Models.Holiday holiday = _context.Holidays.Find(eventId);


                _context.Remove(holiday);
                _context.SaveChanges();
            }
            if (eventType == 1)
            {
                DayOff.Models.WorkDay workDay = _context.WorkDays.Find(eventId);


                _context.Remove(workDay);
                _context.SaveChanges();
            }

        }

        // GET api/DatesTable/DeleteEvent/holidayID 
        [HttpGet("DeleteEvent/{eventId}/{eventType}")]
        public IActionResult DeleteEvent([FromRoute] int eventId, [FromRoute] int eventType)
        {
            string result = "Delete failed.";

            try
            {
                deleteIntEvent(eventId, eventType);
            }

            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }
            result = "Deleted ok.";
            return Ok(JsonUtils.ConvertJsonStr(result));
            ;
        }


        private string createIntEvent(int eventType, string dateStr, int userId, int duration, int type)
        {
            string result;
            DateTime eventDate;
            if (eventType == 0)
            {
                eventDate = DateTime.Parse(dateStr);
                if (countDaysOk(eventDate, userId, duration, type))
                {
                    result = "To many days off taken this week.";
                    return result;
                }
                 if (countHolidaysOk(eventDate, userId, duration, type))
               {
                   result = "To many holidays taken in contract period.";
                   return result;
               }
                if (staffCountOk(eventDate, duration, type, true))
                {
                    result = "To many staff off in the morning of this day.";
                    return result;
                }
                if (staffCountOk(eventDate, duration, type, false))
                {
                    result = "To many staff off in the afternoon of this day.";
                    return result;
                }
              


                DayOff.Models.Holiday holiday = new Holiday();
                holiday.HolType = (HolTypes)type;
                holiday.Duration = (Durations)duration;
                holiday.UserID = userId;
                holiday.HolDate = eventDate;


                _context.Add(holiday);
                _context.SaveChanges();
            }
            if (eventType == 1)
            {
                eventDate = DateTime.Parse(dateStr);

                DayOff.Models.WorkDay workDay = new WorkDay();
                workDay.WorkType = (WorkTypes)type;
                workDay.Duration = (Durations)duration;
                workDay.UserID = userId;
                workDay.WorkDate = eventDate;


                _context.Add(workDay);
                _context.SaveChanges();
            }
            return "Created ok.";
        }

        // GET api/DatesTable/UpdateHoliday/userId/type/duration/date 
        [HttpGet("CreateEvent/{userId}/{type}/{duration}/{dateStr}/{eventType}")]
        public IActionResult CreateEvent([FromRoute] int userId, [FromRoute] int type, [FromRoute] int duration, [FromRoute] string dateStr, [FromRoute] int eventType)
        {
            string result = "";



            try
            {
                result = createIntEvent(eventType, dateStr, userId, duration, type);
                return Ok(JsonUtils.ConvertJsonStr(result));

            }
            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }

        }

        // GET api/DatesTable/UpdateHoliday/userId/type/duration/date 
        [HttpGet("SwapEvent/{userId}/{type}/{duration}/{dateStr}/{eventType}/{oldEventType}/{eventId}")]
        public IActionResult SwapEvent([FromRoute] int userId, [FromRoute] int type, [FromRoute] int duration, [FromRoute] string dateStr, [FromRoute] int eventType, [FromRoute] int oldEventType, [FromRoute] int eventId)
        {
            string result = "";



            try
            {
                deleteIntEvent(eventId, oldEventType);
                result = createIntEvent(eventType, dateStr, userId, duration, type);
                return Ok(JsonUtils.ConvertJsonStr(result));

            }
            catch (Exception e) { return Ok(JsonUtils.ConvertJsonStr(result + " : " + e.Message)); }

        }


        // GET api/DatesTable/UpdateEvent/id/type/duration 
        [HttpGet("UpdateEvent/{id}/{type}/{duration}/{eventType}")]
        public IActionResult UpdateEvent([FromRoute] int id, [FromRoute] int type, [FromRoute] int duration, [FromRoute] int eventType)
        {
            string result = "Update failed.";
            if (id == -1)
            {
                return Ok(JsonUtils.ConvertJsonStr("Update failed. HolidayID=-1"));

            }

            try
            {
                if (eventType == 0)
                {
                    DayOff.Models.Holiday origHoliday = _context.Holidays.Find(id);
                    int userId = origHoliday.UserID;
                    DateTime holDate = origHoliday.HolDate;
                    int origType = (int)origHoliday.HolType;
                    int origDuration = (int)origHoliday.Duration;
                    _context.Remove(origHoliday);
                    _context.SaveChanges();
                    DayOff.Models.Holiday holiday = new Holiday();
                    holiday.HolType = (HolTypes)origType;
                    holiday.Duration = (Durations)origDuration;
                    holiday.UserID = userId;
                    holiday.HolDate = holDate;


                    if (countDaysOk(holiday.HolDate, holiday.UserID, duration, type))
                    {
                        _context.Add(holiday);
                        _context.SaveChanges();
                        result = "To many days off selected.";
                        return Ok(JsonUtils.ConvertJsonStr(result));
                    }
                    if (staffCountOk(holiday.HolDate, duration, type, true))
                    {
                        _context.Add(holiday);
                        _context.SaveChanges();
                        result = "To many staff off in the morning of this day.";
                        return Ok(JsonUtils.ConvertJsonStr(result));
                    }
                    if (staffCountOk(holiday.HolDate, duration, type, false))
                    {
                        _context.Add(holiday);
                        _context.SaveChanges();
                        result = "To many staff off in the afternoon of this day.";
                        return Ok(JsonUtils.ConvertJsonStr(result));
                    }
                    /* if (countHolidaysOk(holDate, userId, duration, type))
                     {
                         result = "To many holidays selected.";
                         return Ok(JsonUtils.ConvertJsonStr(result));
                     }*/
                    holiday.HolType = (HolTypes)type;
                    holiday.Duration = (Durations)duration;
                    _context.Add(holiday);
                    _context.SaveChanges();
                }
                if (eventType == 1)
                {
                    DayOff.Models.WorkDay origWorkDay = _context.WorkDays.Find(id);
                    int userId = origWorkDay.UserID;
                    DateTime workDate = origWorkDay.WorkDate;
                    int origType = (int)origWorkDay.WorkType;
                    int origDuration = (int)origWorkDay.Duration;
                    _context.Remove(origWorkDay);
                    _context.SaveChanges();
                    DayOff.Models.WorkDay workDay = new WorkDay();
                    workDay.WorkType = (WorkTypes)origType;
                    workDay.Duration = (Durations)origDuration;
                    workDay.UserID = userId;
                    workDay.WorkDate = workDate;


                    workDay.WorkType = (WorkTypes)type;
                    workDay.Duration = (Durations)duration;
                    _context.Add(workDay);
                    _context.SaveChanges();
                }


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
