using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

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
    public Guid RoomId { get; set; }

    public Room Room { get; set; }
}
