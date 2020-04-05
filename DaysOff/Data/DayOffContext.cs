using DayOff.Models;
using DaysOff.Models;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<DishDay> DishDays { get; set; }

        public DbSet<RotaDay> RotaDays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Holiday>().ToTable("Holidays");
            modelBuilder.Entity<WorkDay>().ToTable("WorkDays");
            modelBuilder.Entity<DishDay>().ToTable("DishDays");
            modelBuilder.Entity<RotaDay>().ToTable("RotaDays");
        }


    }
}
