using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{

    public enum EventTypes
    {
        Holiday, Work
    }
    public enum Durations
    {
        AM, PM, DO
    }
    public class EventBase
    {
        public int EventID { get; set; }
        public int UserID { get; set; }
        public EventBase(int id)
        {
            EventID = id;
        }
        

        public EventTypes? EventType { get; set; }
        public Durations? Duration { get; set; }
        public DateTime EventDate { get; set; }

    }
}
