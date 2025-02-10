using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class DeviceTypeEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<DeviceType>
{
    protected override void ConfigureEntity(EntityTypeBuilder<DeviceType> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.DefaultConfig)
            .IsRequired()
            .HasColumnOrder(2); 
        
        builder.HasIndex(x => new {x.Name})
            .IsUnique()
            .HasDatabaseName("UX_Name")
            .HasFilter(null);
    }
}