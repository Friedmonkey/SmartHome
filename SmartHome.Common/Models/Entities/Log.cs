using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartHome.Common.Models.Enums;
namespace SmartHome.Common.Models.Entities;
public class Log : Entity
{
    [Required]
    public LogType? Type { get; set; }

    [Required]
    [Column(TypeName = "longtext")]
    public string? Action { get; set; }

    [Required]
    [Column(TypeName = "datetime(6)")]

    public DateTime CreateOn { get; set; }

    [Required]
    public Guid SmartUserId { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    public SmartHomeModel? SmartHome { get; set; }
}
