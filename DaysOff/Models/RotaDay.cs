using DayOff.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Models
{
    public class RotaDay
    {
        [Key]
        public int RotaID { get; set; }
        public int UserID { get; set; }


        public DateTime RotaDate { get; set; }

        public User User { get; set; }
    }
}

