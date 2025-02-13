using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class Home : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? Name { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? SSID { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? SSPassword { get; set; }
}
