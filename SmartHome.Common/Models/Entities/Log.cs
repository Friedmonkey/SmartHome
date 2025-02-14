using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace SmartHome.Common.Models.Entities;
public class Log : Entity
{
    [Required]
    [Column(TypeName = "varchar(60)")]
    public string? Type { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? Action { get; set; }

    [Required]
    [Column(TypeName = "datetime(6)")]
    public DateTime CreateOn { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    public SmartHomeModel? SmartHome { get; set; }
}
