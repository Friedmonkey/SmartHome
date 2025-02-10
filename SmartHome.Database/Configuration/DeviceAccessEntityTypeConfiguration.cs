using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class AccountEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<DeviceAccess>
{
    protected override void ConfigureEntity(EntityTypeBuilder<DeviceAccess> builder)
    {
        builder
            .Property(x => x.DeviceId)
            .IsRequired()
            .HasColumnOrder(1);

        builder
            .Property(x => x.SmartUserId)
            .IsRequired()
            .HasColumnOrder(2); 

        builder.HasIndex(x => new {x.DeviceId, x.SmartUserId})
            .IsUnique()
            .HasDatabaseName("UX_Device_SmartUser")
            .HasFilter(null);
    }
}