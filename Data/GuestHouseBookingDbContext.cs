using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GuestHouseBookingApplication.Models.Entities;
using GuestHouseBookingApplication.Models.Enums;

namespace GuestHouseBookingApplication.Data
{
    public class GuestHouseBookingDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public GuestHouseBookingDbContext(DbContextOptions<GuestHouseBookingDbContext> options)
            : base(options) { }

        public DbSet<GuestHouse> GuestHouses { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Booking: enforce required relationships for approval logic
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.GuestHouse)
                .WithMany(gh => gh.Bookings)
                .HasForeignKey(b => b.GuestHouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Booking>()
                .HasOne(b => b.Bed)
                .WithMany(bed => bed.Bookings)
                .HasForeignKey(b => b.BedId)
                .OnDelete(DeleteBehavior.Restrict);

            // Notification: each notification is for a user
            builder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // AuditLog: track changes per user and entity
            builder.Entity<AuditLog>()
                .HasOne(al => al.User)
                .WithMany(u => u.AuditLogs)
                .HasForeignKey(al => al.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            // Room: each room belongs to one guest house
            builder.Entity<Room>()
                .HasOne(r => r.GuestHouse)
                .WithMany(gh => gh.Rooms)
                .HasForeignKey(r => r.GuestHouseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bed: each bed belongs to one room
            builder.Entity<Bed>()
                .HasOne(bed => bed.Room)
                .WithMany(r => r.Beds)
                .HasForeignKey(bed => bed.RoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique constraint for BedNumber within a Room
            builder.Entity<Bed>()
                .HasIndex(b => new { b.RoomId, b.BedNumber })
                .IsUnique();

            // Add more Fluent API rules here as new features evolve
        }
    }
}