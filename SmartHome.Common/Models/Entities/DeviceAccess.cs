using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public class DeviceAccess : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "varchar(60)")]
    public string Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string DeviceId { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string SmartUserId { get; set; }
}
