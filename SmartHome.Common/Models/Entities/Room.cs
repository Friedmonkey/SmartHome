using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public class Room : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? Name { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    public SmartHomeModel? SmartHome { get; set; }
}
