using DayOff.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DaysOff.Models
{
    public class DishDay
    {

        [Key]
        public int DishID { get; set; }
        public int UserID { get; set; }

       
        public DateTime DishDate { get; set; }

        public User User { get; set; }
    }
}
