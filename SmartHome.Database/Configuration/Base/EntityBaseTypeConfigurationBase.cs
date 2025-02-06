﻿using SmartHome.Database.Entities;

namespace SmartHome.Database.Configuration.Base;

public abstract class EntityBaseTypeConfigurationBase<T>
    : IEntityTypeConfiguration<T>
    where T : EntityBase
{
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasColumnOrder(0);

        ConfigureEntity(builder);

        builder
            .HasKey(x => x.Id)
            .HasName($"PK_{TableName}");

        builder.ToTable(TableName);

    }

    protected virtual string TableName { get; } = typeof(T).Name;

    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}