using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public class Account : Entity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(TypeName = "varchar(60)")]
    public string Id { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string Email { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string PasswordHashed { get; set; }

    [Required]
    [Column(TypeName = "varchar(20)")]
    public string SecurityStamp { get; set; }
}