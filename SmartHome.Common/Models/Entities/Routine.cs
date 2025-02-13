using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class Routine : Entity
{
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string? Name { get; set; }

    [Required]
    [Column(TypeName = "datetime(6)")]
    public DateTime Start { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    [Required]
    [Column(TypeName = "binary(7)")]
    public byte RepeatDays { get; set; }

    public SmartHome SmartHome { get; set; }
    
}
