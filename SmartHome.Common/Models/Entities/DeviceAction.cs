using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class DeviceAction : Entity
{
    [Required]
    [Column(TypeName = "longtext")]
    public string JsonObjectConfig { get; set; }

    [Required]
    public Guid RoutineId { get; set; }
    
    [Required]
    public Guid DeviceId { get; set; }

    public Routine? Routine {  get; set; }

    public Device? Device { get; set; }
    
}
