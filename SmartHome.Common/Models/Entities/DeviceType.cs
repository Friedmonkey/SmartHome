using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace SmartHome.Common.Models.Entities;

public class DeviceType : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? JsonObjectConfig { get; set; }
}