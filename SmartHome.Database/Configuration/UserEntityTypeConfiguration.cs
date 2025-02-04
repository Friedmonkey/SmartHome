using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class UserEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<User>
{
    protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasColumnOrder(1);

        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder.HasIndex(x => new { x.Name, x.DeletedOn })
            .IsUnique()
            .HasDatabaseName("UX_Customer_Name")
            .HasFilter(null);
    }
}