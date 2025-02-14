using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartHome.Common.Models.Entities;

public abstract class EntityBase : IEntityBase
{
    [Key]
    public Guid Id { get; set; }
}

public abstract class Entity : EntityBase
{ }