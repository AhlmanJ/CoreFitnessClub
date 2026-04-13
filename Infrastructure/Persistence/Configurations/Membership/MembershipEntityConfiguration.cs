using Domain.Entities.Membership;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Membership;

internal class MembershipEntityConfiguration : IEntityTypeConfiguration<MembershipEntity>
{
    public void Configure(EntityTypeBuilder<MembershipEntity> builder)
    {
        builder.ToTable("Memberships");

        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Id);

        builder.Property(x => x.MemberId)
                .IsRequired();

        builder.HasIndex(x => x.MemberId);

        builder.Property(x => x.MembershipPlanId)
                .IsRequired();

        builder.Property(x => x.StartDate)
                .IsRequired();

        builder.Property(x => x.EndDate)
                .IsRequired();

        builder.Property(x => x.UpdatedAt);

        builder.Property(x => x.CancelledAt);

        builder.HasOne(x => x.MembershipPlan)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.MembershipPlanId)
                .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Member)
                .WithMany(x => x.Memberships)
                .HasForeignKey(x => x.MemberId)
                .OnDelete(DeleteBehavior.Cascade);       
    }
}
