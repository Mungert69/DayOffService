using DayOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Models
{
    public class HolidayWithUserName : Holiday
    {

        public string UserName { get; set; }
    }
}
