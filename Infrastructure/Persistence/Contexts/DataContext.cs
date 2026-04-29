using Infrastructure.Identity;
using Infrastructure.Entities.Members;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Entities.Booking;
using Infrastructure.Entities.Membership;
using Infrastructure.Entities.TrainingSession;
using Infrastructure.Entities.MembershipPlan;

namespace Infrastructure.Persistence.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        ConfigureRowVersion(modelBuilder);
    }


    /* ---- Code by chatGPT! ----- 
     * I had problem after adding RowVersion to my entity configuration because "Development" enironment was not working.
     * I didn't know how to solve this and therefore had to ask chatGPT.
     */

    private void ConfigureRowVersion(ModelBuilder modelBuilder)
    {
        var isSqlServer = Database.ProviderName ==
                          "Microsoft.EntityFrameworkCore.SqlServer"; // Checks which database is used.

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()) // Loops all Entities in the model
        {
            var property = entityType.FindProperty("RowVersion"); // Searches for prperty named "RowVersion".

            if (property == null)
                continue;

            if (isSqlServer)  // If SQL Database = Update the value of RowVersion.
            {
                property.IsConcurrencyToken = true; 
                property.ValueGenerated =
                    Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAddOrUpdate;
            }
            else // If not SQL Database = ignore the property "RowVersion".
            {
                
                modelBuilder.Entity(entityType.ClrType)
                    .Ignore("RowVersion");
            }
        }
    }

    //--------------------- Code by chatgpt - END ---------------------------------------------

    // Here I name my Entities that I have in Persistence/Configurations.
    public DbSet<MemberEntity> Members => Set<MemberEntity>();
    public DbSet<BookingsEntity> Bookings => Set<BookingsEntity>();
    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();
    public DbSet<MembershipPlanEntity> MemberPlans => Set<MembershipPlanEntity>();
    public DbSet<TrainingSessionEntity> TrainingSessions => Set<TrainingSessionEntity>();
}
