using System;

namespace DaysOff.Objects
{

    public enum EventTypes
    {
        Holiday, Work
    }
    public enum Durations
    {
        AM=0, PM=1
    }
    public class EventBase
    {
        public int EventID { get; set; }
        public int UserID { get; set; }

        public EventBase() { }  
        public EventBase(int id)
        {
            EventID = id;
        }


        public EventTypes? EventType { get; set; }
        public Durations? Duration { get; set; }
        public DateTime EventDate { get; set; }

    }
}
