
// Here i got help from chatGPT on how to create a relationship so that a Member can be a trainer.

using Infrastructure.Entities.TrainingSession;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.TrainingSession;

internal class TrainingSessionEntityConfiguration : IEntityTypeConfiguration<TrainingSessionEntity>
{
    public void Configure(EntityTypeBuilder<TrainingSessionEntity> builder)
    {
        builder.ToTable("TrainingSession");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.CreatedAt)
                .IsRequired();

        builder.HasMany(x => x.Bookings)
            .WithOne(x => x.TrainingSession)
            .HasForeignKey(x => x.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.TrainerMember)
                .WithMany()
                .HasForeignKey(x => x.TrainerMemberId);
    }
}
