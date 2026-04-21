using Infrastructure.Entities.MembershipPlan;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.MembershipPlan;

internal class MembershipPlanEntityConfiguration : IEntityTypeConfiguration<MembershipPlanEntity>
{
    public void Configure(EntityTypeBuilder<MembershipPlanEntity> builder)
    {
        builder.ToTable("MembershipPlan");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id);

        builder.Property(x => x.Name)
                .IsRequired();

        builder.Property(x => x.Description)
                .IsRequired();

        builder.Property(x => x.Price)
                .IsRequired();

        builder.Property(x => x.ValidDays)
                .IsRequired();
    }
}
