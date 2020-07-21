using DaysOff.Controllers;
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using DayOff.Data;
using Microsoft.EntityFrameworkCore;
using DayOff.Models;
using DaysOff.Utils;
using DaysOff.Objects;
using System.Linq;

namespace DaysOff.Controllers.Tests
{
    public class DatesTableControllerTests
    {
       

        [Fact()]
        public void WeekDataTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetDurationsTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetUsersTest()


        {
            

        }

        [Fact()]
        public void GetHolTypesTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetWorkTypesTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetUserTypesTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void ActiveUsersTest()
        {
            DateTime startDate = DateTime.Parse("2020-01-01");
            DateTime endDate = DateTime.Parse("2020-03-01");
            DateTime startDateOver = DateTime.Parse("2020-02-01");
            var data = new List<User> {
                new User { LastName = "user1", StartDate = startDate, EndDate = endDate },
                new User { LastName = "user2", StartDate = startDateOver, EndDate = endDate }
            }.AsQueryable();

            var options = new DbContextOptionsBuilder<DayOffContext>()
           .UseInMemoryDatabase(databaseName: "TestDatabase")
           .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new DayOffContext(options))
            {
                context.Users.Add(new User { LastName = "user1", StartDate = startDate, EndDate = endDate });
                context.Users.Add(new User { LastName = "user2", StartDate = startDateOver, EndDate = endDate });
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            List<IUserBase> users;
            List<IUserBase> usersLimited;
            using (var context = new DayOffContext(options)) {

                users = DataBaseHelper.getActiveUsers(startDate, endDate, context);
                usersLimited = DataBaseHelper.getActiveUsers(startDate.AddDays(10), startDate.AddDays(20), context);
            }

            Assert.Equal(2, users.Count);
            Assert.Single(usersLimited);
        }

        [Fact()]
        public void AllUsersTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void GetTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void PostTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void DeleteEventTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void CreateEventTest()
        {

            var mockSet = new Mock<DbSet<User>>();

            var mockContext = new Mock<DayOffContext>();
            mockContext.Setup(m => m.Users).Returns(mockSet.Object);

            // ToDo add User object to mockSet

            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
            mockContext.Verify(m => m.SaveChanges(), Times.Once());

            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void SwapEventTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void UpdateEventTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void PutTest()
        {
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public void DeleteTest()
        {
            Assert.True(false, "This test needs an implementation");
        }
    }
}