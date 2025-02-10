using SmartHome.Database.Configuration.Base;

namespace SmartHome.Database.Configuration;

public class SmartHomeEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Entities.SmartHome>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Entities.SmartHome> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.SSID)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2); 
        
        builder
            .Property(x => x.SSPassword)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder.HasIndex(x => new {x.Name})
            .IsUnique()
            .HasDatabaseName("UX_Name")
            .HasFilter(null);
    }
}