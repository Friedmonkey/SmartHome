using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class RoutineEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Routine>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Routine> builder)
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
        
        builder
            .Property(x => x.PasswordHashed)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder
            .Property(x => x.SecurityStamp)
            .IsRequired()
            .HasColumnOrder(4);

        builder.HasIndex(x => new {x.Email})
            .IsUnique()
            .HasDatabaseName("UX_Email")
            .HasFilter(null);
    }
}