using DayOff.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class UserDataRow
    {

        private UserBase user;
        private List<EventBase> userRow;

        public UserBase User { get => user; set => user = value; }
        public List<EventBase> UserRow { get => userRow; set => userRow = value; }
    }
}
