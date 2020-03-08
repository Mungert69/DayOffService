using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static DaysOff.Objects.UserBase;

namespace DayOff.Models
{
    public class User
    {


        public int ID { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Used Name")]
        public string FirstName { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Holiday 1/2 days")]
        public int noHolidays { get; set; }

        [Display(Name = "Weekly 1/2 Days Off")]
        public int noHalfDaysOff { get; set; }

        [Display(Name = "User Type")]
        public UserTypes? UserType { get; set; }

        public ICollection<Holiday> Holidays { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }

    }
}
