using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class AccountEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Account>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Account> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(2); 
        
        builder
            .Property(x => x.PasswordHashed)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder
            .Property(x => x.SecurityStamp)
            .IsRequired()
            .HasColumnOrder(3);

        builder.HasIndex(x => new {x.Email})
            .IsUnique()
            .HasDatabaseName("UX_Email")
            .HasFilter(null);
    }
}