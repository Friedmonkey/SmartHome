namespace SmartHome.Database.Entities;

public abstract class EntityBase
    : _IAuditable, IAuditable, IDeletable
{
    public Guid Id { get; set; }
    public DateTimeOffset CreatedOn { get; }
    public DateTimeOffset ModifiedOn { get; }
    public DateTimeOffset? DeletedOn { get; }

    public Guid GetId() => Id;
}

public abstract class Entity
    : EntityBase
{ }