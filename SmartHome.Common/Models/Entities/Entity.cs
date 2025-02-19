using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public abstract class EntityBase : IEntityBase
{
    [Key]
    public Guid Id { get; set; }
}

public abstract class Entity : EntityBase
{ }