using DayOff.Models;
using System;
using System.Linq;



namespace DayOff.Data
{
    public static class DbInitializer
    {
        public static void Initialize(DayOffContext context)
        {
            context.Database.EnsureCreated();

            // Look for any users
            if (context.Users.Any())
            {
                //context.Users.Clear();
                //context.SaveChanges();
                return;   // DB has been seeded
            }

            User[] users = new User[]
            {
            new User{FirstName="MahaDeva",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=3},
            new User{FirstName="Khalsa",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=3},
            new User{FirstName="Adam",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=3},
            new User{FirstName="Supriti",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=3},
            new User{FirstName="Bob",LastName="Dylan",StartDate=DateTime.Now.AddYears(-30), EndDate=DateTime.Now.AddYears(-29),noHolidays=12,noHalfDaysOff=3},
            new User{FirstName="Anna",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=2},
            new User{FirstName="Dganit",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=2},
            new User{FirstName="Shakti",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=2},
            new User{FirstName="Daniela",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999),noHolidays=12,noHalfDaysOff=2},

            };
            foreach (User s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();


        }
    }
}
