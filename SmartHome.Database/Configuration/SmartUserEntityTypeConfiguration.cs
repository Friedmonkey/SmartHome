using SmartHome.Database.Configuration.Base;

namespace SmartHome.Database.Configuration;

public class SmartUserEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<SmartUser>
{
    protected override void ConfigureEntity(EntityTypeBuilder<SmartUser> builder)
    {
        builder
            .Property(x => x.Role)
            .IsRequired()
            .HasColumnOrder(1);

        builder
            .Property(x => x.SmartHomeId)
            .IsRequired()
            .HasColumnOrder(2);

        builder
            .Property(x => x.AccountId)
            .IsRequired()
            .HasColumnOrder(3);

        builder.HasIndex(x => new { x.SmartHomeId, x.AccountId })
           .IsUnique()
           .HasDatabaseName("UX_SmartHome_Account")
           .HasFilter(null);
    }
}