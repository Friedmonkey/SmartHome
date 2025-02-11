using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace SmartHome.Common.Models.Entities;
public class Device : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "varchar(60)")]
    public string Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "longtext")]
    public string JsonObjectConfig { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string RoomGuid { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string RoleGuid { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string SmartHomeId { get; set; }
}
