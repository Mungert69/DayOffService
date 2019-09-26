using DayOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class UserDataRow
    {

        private User user;
        private List<string> userRow;

        public User User { get => user; set => user = value; }
        public List<string> UserRow { get => userRow; set => userRow = value; }
    }
}
