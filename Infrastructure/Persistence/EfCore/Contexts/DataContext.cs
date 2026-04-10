using Infrastructure.Persistence.EfCore.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EfCore.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<ContactRequestEntity> ContactRequests => Set<ContactRequestEntity>();
    public DbSet<GymClassEntity> GymClasses => Set<GymClassEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        modelBuilder.Entity<GymClassEntity>(entity =>
        {
            entity.HasKey(e => e.Id); 
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Instructor).IsRequired();
            entity.ToTable("GymClasses");
        });

        modelBuilder.Entity<BookingEntity>(entity =>
        {
            entity.HasKey(b => new { b.UserId, b.GymClassId });
            entity.ToTable("Bookings");
        });
    }
    public DbSet<BookingEntity> Bookings { get; set; }

    public DbSet<MembershipEntity> Memberships { get; set; }
}