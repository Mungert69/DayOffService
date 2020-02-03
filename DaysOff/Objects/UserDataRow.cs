using System.Collections.Generic;

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
