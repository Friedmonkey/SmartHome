using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class Routine : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? Name { get; set; }
    
    [Required]
    [Column(TypeName = "time")]
    public TimeOnly Start { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    [Required]
    [Column(TypeName = "binary(8)")]
    public byte RepeatDays { get; set; }

    public SmartHomeModel? SmartHome { get; set; }
    public List<DeviceAction>? DeviceActions { get; set; }
    
}
