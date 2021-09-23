using System;
using System.Configuration;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelManagementSystem.Models
{
    public partial class HMSdbContext : DbContext
    {
        public HMSdbContext()
        {
        }

        public HMSdbContext(DbContextOptions<HMSdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<FoodOption> FoodOption { get; set; }
        public virtual DbSet<Guest> Guest { get; set; }
        public virtual DbSet<Reservation> Reservation { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<RoomStatus> RoomStatus { get; set; }
        public virtual DbSet<ShiftSchedule> ShiftSchedule { get; set; }
        public virtual DbSet<SupportTicket> SupportTicket { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.Property(e => e.EmpId).HasColumnName("EmpID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                   

                entity.Property(e => e.EmpFromDate).HasColumnType("date");

                entity.Property(e => e.EmpToDate).HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileAppId)
                    .HasColumnName("MobileAppID")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FoodOption>(entity =>
            {
                entity.Property(e => e.FoodOptionId).HasColumnName("FoodOptionID");

                entity.Property(e => e.FoodOption1)
                    .HasColumnName("FoodOption")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.Property(e => e.GuestId).HasColumnName("GuestID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reservation>(entity =>
            {
                entity.Property(e => e.ReservationId).HasColumnName("ReservationID");

                entity.Property(e => e.FkEmpId).HasColumnName("FK_EmpID");

                entity.Property(e => e.FkFoodOptionId).HasColumnName("FK_FoodOptionID");

                entity.Property(e => e.FkGuestId).HasColumnName("FK_GuestID");

                entity.Property(e => e.FkRoomId).HasColumnName("FK_RoomID");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.FkEmp)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.FkEmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_Employee");

                entity.HasOne(d => d.FkFoodOption)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.FkFoodOptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_FoodOption");

                entity.HasOne(d => d.FkGuest)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.FkGuestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_Guest");

                entity.HasOne(d => d.FkRoom)
                    .WithMany(p => p.Reservation)
                    .HasForeignKey(d => d.FkRoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reservation_Room");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("RoomID");

                entity.Property(e => e.FkRoomStatusId).HasColumnName("FK_RoomStatusID");

                entity.Property(e => e.RoomType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.FkRoomStatus)
                    .WithMany(p => p.Room)
                    .HasForeignKey(d => d.FkRoomStatusId)
                    .HasConstraintName("FK_Room_RoomStatus");
            });

            modelBuilder.Entity<RoomStatus>(entity =>
            {
                entity.Property(e => e.RoomStatusId)
                    .HasColumnName("RoomStatusID")
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<ShiftSchedule>(entity =>
            {
                entity.HasKey(e => e.ShiftId);

                entity.Property(e => e.ShiftId)
                    .HasColumnName("ShiftID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.EmpId).HasColumnName("EmpID");

                entity.Property(e => e.FromDate).HasColumnType("date");

                entity.Property(e => e.ToDate).HasColumnType("date");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.ShiftSchedule)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShiftSchedule_Employee");
            });

            modelBuilder.Entity<SupportTicket>(entity =>
            {
                entity.HasKey(e => e.TicketId);

                entity.Property(e => e.TicketId).ValueGeneratedOnAdd();

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasColumnType("text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
