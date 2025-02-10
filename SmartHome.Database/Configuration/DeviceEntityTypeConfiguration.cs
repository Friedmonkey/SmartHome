using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class DeviceEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Device>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Device> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.DeviceTypeId)
            .IsRequired()
            .HasColumnOrder(2); 
        
        builder
            .Property(x => x.Config)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder
            .Property(x => x.RoomId)
            .IsRequired()
            .HasColumnOrder(4);

        builder.HasIndex(x => new {x.Name})
            .IsUnique()
            .HasDatabaseName("UX_Name")
            .HasFilter(null);
    }
}