using DayOff.Models;
using DaysOff.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DayOff.Models
{
    public class DishDay
    {

        [Key]
        public int DishID { get; set; }
        public int UserID { get; set; }

        public Durations? Duration { get; set; }


        public DateTime DishDate { get; set; }

        public User User { get; set; }
    }
}
