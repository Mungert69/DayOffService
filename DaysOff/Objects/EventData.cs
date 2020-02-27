using DaysOff.Models.LeelaBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class EventData
    {

        private List<Events> eventItems;
        private int[] dayCount;

        public int[] DayCount { get => dayCount; set => dayCount = value; }
        public List<Events> EventItems { get => eventItems; set => eventItems = value; }
    }
}
