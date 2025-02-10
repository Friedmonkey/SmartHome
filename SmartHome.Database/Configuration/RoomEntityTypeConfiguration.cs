using SmartHome.Database.Configuration.Base;

namespace SmartHome.Database.Configuration;

public class RoomEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Room>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Room> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.SmartHomeId)
            .IsRequired()
            .HasColumnOrder(2); 

        builder.HasIndex(x => new {x.Name, x.SmartHomeId})
            .IsUnique()
            .HasDatabaseName("UX_Name_SmartHome")
            .HasFilter(null);
    }
}