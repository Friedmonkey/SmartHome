using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartHome.Common.Models.Enums;
using SmartHome.Common.Models.Configs;

namespace SmartHome.Common.Models.Entities;
public class Device : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "longtext")]
    public string JsonObjectConfig { get; set; }

    [Required]
    public DeviceType Type { get; set; }

    public Guid RoomId { get; set; }

    public Room Room { get; set; }

    [NotMapped,  Newtonsoft.Json.JsonIgnore, System.Text.Json.Serialization.JsonIgnore]
    public DeviceConfig Config { get; set; }
}
