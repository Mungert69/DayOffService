using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Objects
{
    public class DayWorkObj
    {

        private string ohcAm;
        private string ohcPm;
        private string mcAm;
        private string mcPm;
        private string dayOffWeek;

        public string OhcAm { get => ohcAm; set => ohcAm = value; }
        public string OhcPm { get => ohcPm; set => ohcPm = value; }
        public string McAm { get => mcAm; set => mcAm = value; }
        public string McPm { get => mcPm; set => mcPm = value; }
        public string DayOffWeek { get => dayOffWeek; set => dayOffWeek = value; }
    }
}
