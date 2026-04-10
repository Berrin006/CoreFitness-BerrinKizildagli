using Domain.Aggregates.GymClasses;
using Domain.Aggregates.Memberships;
using Infrastructure.Identity;
using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Membership> Memberships { get; set; }
        public DbSet<BookingEntity> Bookings { get; set; } 
        public DbSet<GymClass> GymClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BookingEntity>()
                .HasOne(b => b.GymClass)
                .WithMany()
                .HasForeignKey(b => b.GymClassId);
        }
    }
}