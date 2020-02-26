﻿using DaysOff.Objects;
using System;
using System.ComponentModel.DataAnnotations;

namespace DayOff.Models
{


    public class WorkDay
    {


        [Key]
        public int WorkID { get; set; }
        public int UserID { get; set; }

        public WorkTypes? WorkType { get; set; }
        public Durations? Duration { get; set; }
        public DateTime WorkDate { get; set; }

        public User User { get; set; }



    }

}