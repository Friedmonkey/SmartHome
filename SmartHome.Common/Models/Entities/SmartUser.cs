using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public class SmartUser : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "varchar(60)")]
    public string Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string AccountId { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string SmartHomeId { get; set; }

    [Required]
    [Column(TypeName = "varchar(60)")]
    public string RoleId { get; set; }
}
