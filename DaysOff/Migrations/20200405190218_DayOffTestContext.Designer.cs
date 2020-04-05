﻿// <auto-generated />
using System;
using DayOff.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DaysOff.Migrations
{
    [DbContext(typeof(DayOffContext))]
    [Migration("20200405190218_DayOffTestContext")]
    partial class DayOffTestContext
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DayOff.Models.Holiday", b =>
                {
                    b.Property<int>("HolidayID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Duration");

                    b.Property<DateTime>("HolDate");

                    b.Property<int?>("HolType");

                    b.Property<int>("UserID");

                    b.HasKey("HolidayID");

                    b.HasIndex("UserID");

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("DayOff.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<DateTime>("StartDate");

                    b.Property<int?>("UserType");

                    b.Property<int>("noHalfDaysOff");

                    b.Property<int>("noHolidays");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DayOff.Models.WorkDay", b =>
                {
                    b.Property<int>("WorkID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Duration");

                    b.Property<int>("UserID");

                    b.Property<DateTime>("WorkDate");

                    b.Property<int?>("WorkType");

                    b.HasKey("WorkID");

                    b.HasIndex("UserID");

                    b.ToTable("WorkDays");
                });

            modelBuilder.Entity("DaysOff.Models.DishDay", b =>
                {
                    b.Property<int>("DishID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DishDate");

                    b.Property<int>("UserID");

                    b.HasKey("DishID");

                    b.HasIndex("UserID");

                    b.ToTable("DishDays");
                });

            modelBuilder.Entity("DayOff.Models.Holiday", b =>
                {
                    b.HasOne("DayOff.Models.User", "User")
                        .WithMany("Holidays")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DayOff.Models.WorkDay", b =>
                {
                    b.HasOne("DayOff.Models.User", "User")
                        .WithMany("WorkDays")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DaysOff.Models.DishDay", b =>
                {
                    b.HasOne("DayOff.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
