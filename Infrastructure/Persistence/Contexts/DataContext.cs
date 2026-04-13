using Infrastructure.Identity;
using Domain.Entities.Members;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Booking;
using Domain.Entities.Membership;
using Domain.Entities.Membership.MembershipPlan;
using Domain.Entities.TrainingSession;

namespace Infrastructure.Persistence.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    // Here I name my Entities that I have in Persistence/Configurations.
    public DbSet<MemberEntity> Members => Set<MemberEntity>();
    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();
    public DbSet<MembershipPlanEntity> MemberPlans => Set<MembershipPlanEntity>();
    public DbSet<TrainingSessionEntity> TrainingSessions => Set<TrainingSessionEntity>();
}
