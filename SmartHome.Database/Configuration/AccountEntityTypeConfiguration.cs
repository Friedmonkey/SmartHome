﻿using SmartHome.Database.Configuration.Base;
using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration;

public class RoutineEntityTypeConfiguration
    : EntityBaseTypeConfigurationBase<DeviceAction>
{
    protected override void ConfigureEntity(EntityTypeBuilder<DeviceAction> builder)
    {
        builder
            .Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnOrder(1);

        builder
            .Property(x => x.Config)
            .IsRequired()
            .HasColumnOrder(2); 
        
        builder
            .Property(x => x.DeviceId)
            .IsRequired()
            .HasColumnOrder(3);
        
        builder
            .Property(x => x.RoutineId)
            .IsRequired()
            .HasColumnOrder(4);

        builder.HasIndex(x => new {x.DeviceId, x.RoutineId})
            .IsUnique()
            .HasDatabaseName("UX_Device_Routine")   
            .HasFilter(null);
    }
}