using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DaysOff.Models.LeelaBack
{
    public partial class LeelaBackContext : DbContext
    {
        public LeelaBackContext()
        {
        }

        public LeelaBackContext(DbContextOptions<LeelaBackContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accommodation> Accommodation { get; set; }
        public virtual DbSet<Allocations> Allocations { get; set; }
        public virtual DbSet<BookingExtras> BookingExtras { get; set; }
        public virtual DbSet<Bookings> Bookings { get; set; }
        public virtual DbSet<ContactHistory> ContactHistory { get; set; }
        public virtual DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<CtHmvyInfo> CtHmvyInfo { get; set; }
        public virtual DbSet<EventCategories> EventCategories { get; set; }
        public virtual DbSet<EventPrices> EventPrices { get; set; }
        public virtual DbSet<Events> Events { get; set; }
        public virtual DbSet<Heard> Heard { get; set; }
        public virtual DbSet<Installments> Installments { get; set; }
        public virtual DbSet<LeelaLinks> LeelaLinks { get; set; }
        public virtual DbSet<PaymentMethods> PaymentMethods { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<PriceTypes> PriceTypes { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<SpeedLink> SpeedLink { get; set; }
        public virtual DbSet<TherapistTraining> TherapistTraining { get; set; }
        public virtual DbSet<TrainingCategoryMap> TrainingCategoryMap { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DEVELOP\\OSHOLEELA;Database=LeelaBack;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.HasKey(e => e.AccomId);

                entity.Property(e => e.AccomId)
                    .HasColumnName("AccomID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccomDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<Allocations>(entity =>
            {
                entity.HasKey(e => e.AllocId);

                entity.Property(e => e.AllocId)
                    .HasColumnName("AllocID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AllocAmt).HasColumnType("money");

                entity.Property(e => e.AllocNotes).HasMaxLength(150);

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.PayId).HasColumnName("PayID");
            });

            modelBuilder.Entity<BookingExtras>(entity =>
            {
                entity.HasKey(e => e.Beid);

                entity.Property(e => e.Beid)
                    .HasColumnName("BEID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Benotes)
                    .HasColumnName("BENotes")
                    .HasMaxLength(255);

                entity.Property(e => e.Beprice)
                    .HasColumnName("BEPrice")
                    .HasColumnType("money");

                entity.Property(e => e.Beqty).HasColumnName("BEQty");

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.EpriceId).HasColumnName("EPriceID");
            });

            modelBuilder.Entity<Bookings>(entity =>
            {
                entity.HasKey(e => e.BookingId);

                entity.Property(e => e.BookingId)
                    .HasColumnName("BookingID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AccomId).HasColumnName("AccomID");

                entity.Property(e => e.BkCancelled).HasColumnType("datetime");

                entity.Property(e => e.BkClosed).HasColumnType("datetime");

                entity.Property(e => e.BkDate).HasColumnType("datetime");

                entity.Property(e => e.BkNotes).HasMaxLength(255);

                entity.Property(e => e.BkPrice).HasColumnType("money");

                entity.Property(e => e.CheckedIn).HasColumnName("CheckedIn?");

                entity.Property(e => e.ConfSent).HasColumnType("datetime");

                entity.Property(e => e.CtId).HasColumnName("CtID");

                entity.Property(e => e.CustomA).HasColumnName("CustomA?");

                entity.Property(e => e.CustomB).HasColumnName("CustomB?");

                entity.Property(e => e.EpriceId).HasColumnName("EPriceID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.ExtraDinner).HasColumnName("ExtraDinner?");

                entity.Property(e => e.ExtraNight).HasColumnName("ExtraNight?");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");
            });

            modelBuilder.Entity<ContactHistory>(entity =>
            {
                entity.HasKey(e => e.Chid);

                entity.Property(e => e.Chid)
                    .HasColumnName("CHID")
                    .ValueGeneratedNever();

                entity.Property(e => e.Chdate)
                    .HasColumnName("CHDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.Chdetails)
                    .HasColumnName("CHDetails")
                    .HasMaxLength(150);

                entity.Property(e => e.Chdone)
                    .HasColumnName("CHDone")
                    .HasColumnType("datetime");

                entity.Property(e => e.ChfollowUp)
                    .HasColumnName("CHFollowUp")
                    .HasColumnType("datetime");

                entity.Property(e => e.Confidential).HasColumnName("Confidential?");

                entity.Property(e => e.CtId).HasColumnName("CtID");
            });

            modelBuilder.Entity<Contacts>(entity =>
            {
                entity.HasKey(e => e.CtId);

                entity.Property(e => e.CtId)
                    .HasColumnName("CtID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CtAddress).HasMaxLength(150);

                entity.Property(e => e.CtCreated).HasColumnType("datetime");

                entity.Property(e => e.CtDob)
                    .HasColumnName("CtDOB")
                    .HasColumnType("datetime");

                entity.Property(e => e.CtEmail).HasMaxLength(60);

                entity.Property(e => e.CtHomeTel).HasMaxLength(20);

                entity.Property(e => e.CtMarked).HasColumnName("CtMarked?");

                entity.Property(e => e.CtMobile).HasMaxLength(20);

                entity.Property(e => e.CtNationality).HasMaxLength(25);

                entity.Property(e => e.CtWorkTel).HasMaxLength(20);

                entity.Property(e => e.Dbuser).HasColumnName("DBUser?");

                entity.Property(e => e.EmergencyContact).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(15);

                entity.Property(e => e.HeardId).HasColumnName("HeardID");

                entity.Property(e => e.LastName).HasMaxLength(25);

                entity.Property(e => e.LlinkCode)
                    .HasColumnName("LLinkCode")
                    .HasMaxLength(1);

                entity.Property(e => e.SendEmailings).HasColumnName("SendEmailings?");

                entity.Property(e => e.SendMailings).HasColumnName("SendMailings?");

                entity.Property(e => e.SendTexts).HasColumnName("SendTexts?");

                entity.Property(e => e.Ttref)
                    .HasColumnName("TTRef")
                    .HasMaxLength(4);

                entity.Property(e => e.UsedName).HasMaxLength(15);
            });

            modelBuilder.Entity<CtHmvyInfo>(entity =>
            {
                entity.HasKey(e => e.CtHmvyId);

                entity.Property(e => e.CtHmvyId)
                    .HasColumnName("CtHmvyID")
                    .ValueGeneratedNever();

                entity.Property(e => e.LastHiv)
                    .HasColumnName("LastHIV")
                    .HasColumnType("datetime");

                entity.Property(e => e.NextHiv)
                    .HasColumnName("NextHIV")
                    .HasColumnType("datetime");

                entity.Property(e => e.Y1complete)
                    .HasColumnName("Y1Complete")
                    .HasColumnType("datetime");

                entity.Property(e => e.Y1start)
                    .HasColumnName("Y1Start")
                    .HasColumnType("datetime");

                entity.Property(e => e.Y2complete)
                    .HasColumnName("Y2Complete")
                    .HasColumnType("datetime");

                entity.Property(e => e.Y2start)
                    .HasColumnName("Y2Start")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<EventCategories>(entity =>
            {
                entity.HasKey(e => e.EcatRef);

                entity.Property(e => e.EcatRef)
                    .HasColumnName("ECatRef")
                    .HasMaxLength(3)
                    .ValueGeneratedNever();

                entity.Property(e => e.EcatDesc)
                    .HasColumnName("ECatDesc")
                    .HasMaxLength(25);

                entity.Property(e => e.EcatGuests).HasColumnName("ECatGuests?");

                entity.Property(e => e.Tsequence).HasColumnName("TSequence?");
            });

            modelBuilder.Entity<EventPrices>(entity =>
            {
                entity.HasKey(e => e.EpriceId);

                entity.Property(e => e.EpriceId)
                    .HasColumnName("EPriceID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EpriceAmt)
                    .HasColumnName("EPriceAmt")
                    .HasColumnType("money");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.Ptcode)
                    .HasColumnName("PTCode")
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<Events>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.Property(e => e.EventId)
                    .HasColumnName("EventID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AddChildLetter).HasMaxLength(255);

                entity.Property(e => e.BookingInstructions).HasMaxLength(255);

                entity.Property(e => e.EcatRef)
                    .HasColumnName("ECatRef")
                    .HasMaxLength(3);

                entity.Property(e => e.EventCancelled).HasColumnType("datetime");

                entity.Property(e => e.EventEnd).HasColumnType("datetime");

                entity.Property(e => e.EventName).HasMaxLength(50);

                entity.Property(e => e.EventStart).HasColumnType("datetime");

                entity.Property(e => e.ExtraAcaption)
                    .HasColumnName("ExtraACaption")
                    .HasMaxLength(20);

                entity.Property(e => e.ExtraBcaption)
                    .HasColumnName("ExtraBCaption")
                    .HasMaxLength(20);

                entity.Property(e => e.Qbcode)
                    .HasColumnName("QBCode")
                    .HasMaxLength(12);

                entity.Property(e => e.UseExtraA).HasColumnName("UseExtraA?");

                entity.Property(e => e.UseExtraB).HasColumnName("UseExtraB?");
            });

            modelBuilder.Entity<Heard>(entity =>
            {
                entity.Property(e => e.HeardId)
                    .HasColumnName("HeardID")
                    .ValueGeneratedNever();

                entity.Property(e => e.HeardDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<Installments>(entity =>
            {
                entity.HasKey(e => e.InstId);

                entity.Property(e => e.InstId)
                    .HasColumnName("InstID")
                    .ValueGeneratedNever();

                entity.Property(e => e.BookingId).HasColumnName("BookingID");

                entity.Property(e => e.InstAmount).HasColumnType("money");

                entity.Property(e => e.InstExpected).HasColumnType("datetime");

                entity.Property(e => e.InstNotes).HasMaxLength(255);
            });

            modelBuilder.Entity<LeelaLinks>(entity =>
            {
                entity.HasKey(e => e.LlinkCode);

                entity.Property(e => e.LlinkCode)
                    .HasColumnName("LLinkCode")
                    .HasMaxLength(1)
                    .ValueGeneratedNever();

                entity.Property(e => e.LlinkDesc)
                    .HasColumnName("LLinkDesc")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentMethods>(entity =>
            {
                entity.HasKey(e => e.PmethodCode);

                entity.Property(e => e.PmethodCode)
                    .HasColumnName("PMethodCode")
                    .HasMaxLength(1)
                    .ValueGeneratedNever();

                entity.Property(e => e.PmethodDesc)
                    .HasColumnName("PMethodDesc")
                    .HasMaxLength(30);
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PayId);

                entity.Property(e => e.PayId)
                    .HasColumnName("PayID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CtId).HasColumnName("CtID");

                entity.Property(e => e.PayAmt).HasColumnType("money");

                entity.Property(e => e.PayClosed).HasColumnType("datetime");

                entity.Property(e => e.PayDate).HasColumnType("datetime");

                entity.Property(e => e.PayNotes).HasMaxLength(255);

                entity.Property(e => e.Payer).HasMaxLength(50);

                entity.Property(e => e.PmethodCode)
                    .HasColumnName("PMethodCode")
                    .HasMaxLength(1);
            });

            modelBuilder.Entity<PriceTypes>(entity =>
            {
                entity.HasKey(e => e.Ptcode);

                entity.Property(e => e.Ptcode)
                    .HasColumnName("PTCode")
                    .HasMaxLength(2)
                    .ValueGeneratedNever();

                entity.Property(e => e.GlobalPrice).HasColumnType("money");

                entity.Property(e => e.Ptchild).HasColumnName("PTChild?");

                entity.Property(e => e.Ptdesc)
                    .HasColumnName("PTDesc")
                    .HasMaxLength(30);

                entity.Property(e => e.Ptextra).HasColumnName("PTExtra?");
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId)
                    .HasColumnName("RoleID")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleDesc).HasMaxLength(50);
            });

            modelBuilder.Entity<SpeedLink>(entity =>
            {
                entity.HasKey(e => e.Slid);

                entity.Property(e => e.Slid)
                    .HasColumnName("SLID")
                    .HasMaxLength(1)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<TherapistTraining>(entity =>
            {
                entity.HasKey(e => e.Ttref);

                entity.Property(e => e.Ttref)
                    .HasColumnName("TTRef")
                    .HasMaxLength(4)
                    .ValueGeneratedNever();

                entity.Property(e => e.PayInAdvance).HasColumnName("PayInAdvance?");

                entity.Property(e => e.Ttdesc)
                    .HasColumnName("TTDesc")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<TrainingCategoryMap>(entity =>
            {
                entity.HasKey(e => e.Tcid);

                entity.Property(e => e.Tcid)
                    .HasColumnName("TCID")
                    .ValueGeneratedNever();

                entity.Property(e => e.EcatRef)
                    .HasColumnName("ECatRef")
                    .HasMaxLength(3);

                entity.Property(e => e.Ttref)
                    .HasColumnName("TTRef")
                    .HasMaxLength(4);
            });
        }
    }
}
