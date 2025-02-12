namespace SmartHome.Common.Models.Entities;

public abstract class EntityBase : IEntityBase
{
    public Guid Id { get; set; }
    public Guid GetId() => Id;
}

public abstract class Entity : EntityBase
{ }