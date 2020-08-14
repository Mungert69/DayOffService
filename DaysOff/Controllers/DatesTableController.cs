using DayOff.Data;
using DayOff.Models;
using DaysOff.ExtensionMethods;
using DaysOff.Models.LeelaBack;
using DaysOff.Objects;
using DaysOff.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static DaysOff.Objects.UserBase;

namespace DaysOff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatesTableController : ControllerBase
    {
        private readonly DayOffContext _context;
        private readonly LeelaBackContext _contextLeelaBack;

        public DatesTableController(DayOffContext context, LeelaBackContext contextLeelaBack )
        {
            _context = context;
            _contextLeelaBack = contextLeelaBack;
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

        private List<SelectItem> getUserTypes()
        {
            List<SelectItem> types = new List<SelectItem>();
            SelectItem selectItem;
            List<UserTypes> userTypes = Enum.GetValues(typeof(UserTypes)).Cast<UserTypes>().ToList();
            foreach (UserTypes userType in userTypes)
            {
                selectItem = new SelectItem();
                selectItem.Label = userType.ToString();
                selectItem.Value = userType.ToString();
                selectItem.Id = Convert.ToInt32(userType);

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

      
        private List<IUserBase> getActiveUsers(DateTime startCheck, DateTime endCheck)
        {
           
            return DataBaseHelper.getActiveUsers(startCheck,  endCheck,_context);
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
               
            }
            return 0;
        }


        private bool staffCountOk(DateTime holDate, int type, int duration)
        {

            if (type == 4) return false;
            List<Holiday> holidays = _context.Holidays.Where(h => h.HolDate == holDate && h.Duration == (Durations)duration).ToList();

           
            int count = holidays.Count()+1;
            int staffTotal = getActiveUsers(holDate,holDate).Count();
            if (count <= (staffTotal - 2)) { return false; }
            return true;

        }
        private bool countHolidaysOk(DateTime holDate, int userId, int duration, int type)
        {
            // type 4 is override so no checking for staff days off

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
            List<Holiday> holidays = _context.Holidays.Where(h => h.UserID == userId && h.HolDate >= startDate && h.HolDate <= endDate && h.HolType == HolTypes.D).ToList();
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

            List<UserDataRow> userDataRows = new List<UserDataRow>();
            WeekData weekData = new WeekData();

            headerDates.Add(from);
            for (int i = 1; i < 6; i++)
            {
                headerDates.Add(from.AddDays(i));
            }
            headerDates.Add(to);

            int[] eventCountArray = new int[7];
            int counter = 0;
            List<Events> eventItems = new List<Events>();
            try {
               /* foreach (DateTime headerDate in headerDates)
                {
                    List<Events> events = _contextLeelaBack.Events.Where(e => headerDate >= e.EventStart && headerDate <= e.EventEnd && (e.EventEnd - e.EventStart).Value.TotalDays < 15 && e.EventCancelled == null).ToList();
                    if (events.Count() > 0)
                    {
                        foreach (Events eventItem in events)
                        {
                            if (!eventItems.Contains(eventItem))
                            {
                                eventItems.Add(eventItem);
                            }
                        }
                        eventCountArray[counter] = events.Count();
                    }
                    else
                    {
                        eventCountArray[counter] = 0;
                    }
                    counter++;
                }
                */
            }
            catch (Exception e) {
                string test = "";
            }
           
            EventData eventData = new EventData();
            eventData.EventItems = eventItems;
            eventData.DayCount = eventCountArray;

            List<IUserBase> users = getActiveUsers(from,to);
            List<IUserBase> allUsers = DataBaseHelper.getAllUsers( _context);
            List<DayWorkObj> dayWorkObjs = new List<DayWorkObj>();
            foreach (DateTime date in headerDates)
            {
                dayWorkObjs.Add(WeekDataCreator.getDayWorkObj(date, _context,allUsers));
            }

            foreach (UserBase user in users)
            {
                userRow = new List<EventBase>();
                foreach (DateTime date in headerDates)
                {
                    userRow.Add(WeekDataCreator.getData(date, user, _context, 0));
                }
                UserDataRow userDataRow = new UserDataRow();
                userDataRow.User = user;
                userDataRow.UserRow = userRow;
                userDataRows.Add(userDataRow);

                userRow = new List<EventBase>();
                foreach (DateTime date in headerDates)
                {
                    userRow.Add(WeekDataCreator.getData(date, user, _context, 1));
                }
                userDataRow = new UserDataRow();
                userDataRow.User = user;
                userDataRow.UserRow = userRow;
                userDataRows.Add(userDataRow);
            }

            weekData.DayWorkObjs = dayWorkObjs;
            weekData.HeaderDates = headerDates;
            weekData.UserDataRows = userDataRows;
            weekData.EventData = eventData;
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
        public ActionResult<IEnumerable<IUserBase>> GetUsers()
        {
            return getActiveUsers(DateTime.Now.StartOfWeek(DayOfWeek.Monday), DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(6));
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

        // GET api/DatesTable/GetUserTypes
        [HttpGet("GetUserTypes")]
        public ActionResult<IEnumerable<SelectItem>> GetUserTypes()
        {
            return getUserTypes();
        }

        // GET api/DatesTable/ActiveUsers
        [HttpGet("ActiveUsers")]
        public ActionResult<IEnumerable<IUserBase>> ActiveUsers()
        {
            return getActiveUsers(DateTime.Now,DateTime.Now);
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
        public ActionResult<ResultObj> DeleteEvent([FromRoute] int eventId, [FromRoute] int eventType)
        {
            ResultObj result = new ResultObj();
            result.Message = "Delete failed.";
            result.Success=false;

            try
            {
                deleteIntEvent(eventId, eventType);
            }

            catch (Exception e) { result.Message= result.Message + " : " + e.Message;
                return result;
            }
            result.Message = "Deleted OK.";
            result.Success = true;
            return result;
            ;
        }


        private void createIntEvent(int eventType, string dateStr, int userId, int duration, int type, ResultObj result)
        {
            result.Success= false;
            DateTime eventDate;
            if (eventType == 0)
            {
                
                eventDate = DateTime.Parse(dateStr);
                if (type==1 && countDaysOk(eventDate, userId, duration, type))
                {
                    result.Message = "To many days off taken this week.";
                    return ;
                }
                 if (type==0 && countHolidaysOk(eventDate, userId, duration, type))
               {
                   result.Message = "To many holidays taken in contract period.";
                    return ;
               }

                if (staffCountOk(eventDate, type, duration))
                {
                    if (duration == 0) {
                        result.Message = "To many staff off in the morning of this day.";
                    }
                    else {
                        result.Message = "To many staff off in the afternoon of this day.";
                    }
                    return ;
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
            result.Message= "Created ok.";
            result.Success = true;
        }

        // GET api/DatesTable/UpdateHoliday/userId/type/duration/date 
        [HttpGet("CreateEvent/{userId}/{type}/{duration}/{dateStr}/{eventType}")]
        public ActionResult<ResultObj> CreateEvent([FromRoute] int userId, [FromRoute] int type, [FromRoute] int duration, [FromRoute] string dateStr, [FromRoute] int eventType)
        {
            ResultObj result = new ResultObj();
            result.Message = "Create failed.";
            result.Success = false;

            try
            {
                createIntEvent(eventType, dateStr, userId, duration, type, result);
                return result;

            }
            catch (Exception e) { 
                result.Message= result.Message + " : " + e.Message;
                return result;
            }

        }

        // GET api/DatesTable/UpdateHoliday/userId/type/duration/date 
        [HttpGet("SwapEvent/{userId}/{type}/{duration}/{dateStr}/{eventType}/{oldEventType}/{eventId}")]
        public ActionResult<ResultObj> SwapEvent([FromRoute] int userId, [FromRoute] int type, [FromRoute] int duration, [FromRoute] string dateStr, [FromRoute] int eventType, [FromRoute] int oldEventType, [FromRoute] int eventId)
        {
            ResultObj result = new ResultObj();
            result.Message = "Update Failed.";
            result.Success = false;


            try
            {
                deleteIntEvent(eventId, oldEventType);
                createIntEvent(eventType, dateStr, userId, duration, type, result);
                return result;

            }
            catch (Exception e) {
                result.Message = result.Message + " : " + e.Message;
                return result; }

        }


        // GET api/DatesTable/UpdateEvent/id/type/duration 
        [HttpGet("UpdateEvent/{id}/{type}/{duration}/{eventType}")]
        public ActionResult<ResultObj> UpdateEvent([FromRoute] int id, [FromRoute] int type, [FromRoute] int duration, [FromRoute] int eventType)
        {
            ResultObj result = new ResultObj();
            result.Message = "Update failed.";
            result.Success = false;

            if (id == -1)
            {
                result.Message = "Update failed. HolidayID=-1";
                return result;

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
                    if (type == 1 && countDaysOk(holDate, userId, duration, type))
                    {
                       
                        _context.Add(holiday);
                        _context.SaveChanges();
                        result.Message = "To many days off taken this week.";
                        return result;
                    }
                    if (type == 0 && countHolidaysOk(holDate, userId, duration, type))
                    {
                        _context.Add(holiday);
                        _context.SaveChanges();
                        result.Message = "To many holidays taken in contract period.";
                        return result;
                    }
                    if (staffCountOk(holDate, type, duration))
                    {
                        _context.Add(holiday);
                        _context.SaveChanges();
                        if (duration == 0) {
                            result.Message = "To many staff off in the morning of this day.";

                        }
                        else {
                            result.Message = "To many staff off in the afternoon of this day.";
                        }

                        return result;
                       
                    }
                   
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

            result.Message = "Updated Ok.";
            result.Success = true;
            return result;
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
