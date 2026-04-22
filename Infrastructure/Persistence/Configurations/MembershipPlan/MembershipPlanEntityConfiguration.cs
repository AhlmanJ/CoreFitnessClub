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
                .HasMaxLength(30)
                .IsRequired();

        builder.Property(x => x.Description)
                .HasMaxLength(200)
                .IsRequired();

        builder.Property(x => x.ListItem1)
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(x => x.ListItem2)
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(x => x.ListItem3)
                .HasMaxLength(50)
                .IsRequired();

        builder.Property(x => x.Price)
                .IsRequired();

        builder.Property(x => x.ValidDays)
                .IsRequired();

        builder.Property(x => x.RowVersion)
                .IsRowVersion()
                .IsConcurrencyToken()
                .IsRequired();
    }
}
