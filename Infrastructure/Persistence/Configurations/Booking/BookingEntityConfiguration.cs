using Infrastructure.Entities.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Booking;

internal class BookingEntityConfiguration : IEntityTypeConfiguration<BookingsEntity>
{
    public void Configure(EntityTypeBuilder<BookingsEntity> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.MemberId)
                .IsRequired();

        builder.HasIndex(x => x.MemberId);

        builder.Property(x => x.TrainingSessionId)
                .IsRequired();

        builder.HasIndex(x => x.TrainingSessionId);

        builder.Property(x => x.Status)
                .IsRequired();

        builder.Property(x => x.CreatedAt)
                .IsRequired();

        builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken();

        builder.HasOne(x => x.Member)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.TrainingSession)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.TrainingSessionId)
                .OnDelete(DeleteBehavior.Restrict);
    }
}
