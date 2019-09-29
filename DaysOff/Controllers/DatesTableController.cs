﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DayOff.Data;
using DayOff.Models;
using DaysOff.Objects;
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
        private List<UserBase> getActiveUsers() {
            List<UserBase> users = new List<UserBase>();
            users = _context.Users.Where(u => u.StartDate < DateTime.Now && u.EndDate > DateTime.Now).Select(s => new UserBase(s.ID,s.LastName,s.FirstName,s.StartDate,s.EndDate)).ToList();

            return users;
        }

        // GET api/DatesTable/WeekData
        [HttpGet("WeekData/{fromStr}/{toStr}")]
        public ActionResult<WeekData> WeekData([FromRoute] string fromStr, [FromRoute] string toStr)
        {
            DateTime from;
            DateTime to;
            try {
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
                    var holData= _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date).FirstOrDefault();
                    if (holData == null) {
                        userRow.Add(new HolidayBase(-1));
                    }
                    else {
                        userRow.Add(new HolidayBase(holData.HolidayID,(HolidayBase.HolTypes)holData.HolType,(HolidayBase.Durations)holData.Duration,holData.HolDate));
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

        // GET api/DatesTable/UpdateHoliday/id/date/userid 
        [HttpGet("UpdateHoliday/{id}/{type}/{duration}")]
        public string UpdateHoliday([FromRoute] int id, [FromRoute] int type, [FromRoute] int duration)
        {
            string result = "failed";
            if (id == -1)
            {
                return "";
            }

            DayOff.Models.Holiday holiday =  _context.Holidays.Find(id);
            holiday.HolType = (Holiday.HolTypes)type;
            holiday.Duration = (Holiday.Durations)duration;

            _context.Update(holiday);
             _context.SaveChanges();

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
