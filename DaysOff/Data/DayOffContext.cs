using DayOff.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DayOff.Data
{
    public class DayOffContext : DbContext
    {

        public DayOffContext(DbContextOptions<DayOffContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        public DbSet<WorkDay> WorkDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Holiday>().ToTable("Holidays");
            modelBuilder.Entity<WorkDay>().ToTable("WorkDays");
        }
    

}
}
