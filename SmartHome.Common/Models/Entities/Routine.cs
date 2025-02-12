using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class Routine : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "varchar(60)")]
    public string Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "datetime(6)")]
    public DateTime Start { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string SmartHomeId { get; set; }

    [Required]
    [Column(TypeName = "binary(7)")]
    public byte RepeatDays { get; set; }
    
}
