using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class RoleEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Role>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Role> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.HasIndex(x => new {x.Name})
            .IsUnique()
            .HasDatabaseName("UX_Name")
            .HasFilter(null);
    }
}