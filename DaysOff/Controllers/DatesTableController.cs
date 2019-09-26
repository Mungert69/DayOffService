using System;
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

        private List<User> getActiveUsers() {
            List<User> users = new List<User>();
            users = _context.Users.Where(u => u.StartDate < DateTime.Now && u.EndDate > DateTime.Now).ToList();

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
            List<string> userRow = new List<string>();
            List<User> users = getActiveUsers();
            List<UserDataRow> userDataRows = new List<UserDataRow>();
            WeekData weekData = new WeekData();

            headerDates.Add(from);
            for (int i = 1; i < 6; i++)
            {
                headerDates.Add(from.AddDays(i));
            }
            headerDates.Add(to);

            foreach (User user in users)
            {
                userRow = new List<string>();
                foreach (DateTime date in headerDates)
                {
                    var holData= _context.Holidays.Where(h => h.UserID == user.ID && h.HolDate == date).Select(s => new { s.Duration, s.HolType }).FirstOrDefault();
                    if (holData == null) {
                        userRow.Add("");
                    }
                    else {
                        userRow.Add(holData.Duration + " : " + holData.HolType);
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

        // GET api/DatesTable/ActiveUsers
        [HttpGet("ActiveUsers")]
        public ActionResult<IEnumerable<User>> ActiveUsers()
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
