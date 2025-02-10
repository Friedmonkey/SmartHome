using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class LogEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<Log>
{
    protected override void ConfigureEntity(EntityTypeBuilder<Log> builder)
    {
        builder
            .Property(x => x.LogType)
            .IsRequired()
            .HasColumnOrder(1);

        builder
            .Property(x => x.Action)
            .IsRequired()
            .HasColumnOrder(2); 
        
        builder
            .Property(x => x.CreatedOn)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder
            .Property(x => x.SmartUserId)
            .IsRequired()
            .HasColumnOrder(4);
    }
}