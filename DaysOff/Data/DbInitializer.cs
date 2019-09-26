using DayOff.Models;
using DaysOff.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



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

            var users = new User[]
            {
            new User{FirstName="MahaDeva",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Tom",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Premal",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Khalsa",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Adam",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Supriti",LastName="",StartDate=DateTime.Now.AddDays(-1), EndDate=DateTime.Now.AddYears(999)},
            new User{FirstName="Bob",LastName="Dylan",StartDate=DateTime.Now.AddYears(-30), EndDate=DateTime.Now.AddYears(-29)},

            };
            foreach (User s in users)
            {
                context.Users.Add(s);
            }
            context.SaveChanges();

          
        }
    }
}
