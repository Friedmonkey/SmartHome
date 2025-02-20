using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;
public class SmartHomeModel : Entity
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

    public override bool Equals(object obj)
    {
        if (obj is SmartHomeModel other)
        {
            return Id == other.Id; // Compare by unique ID
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode(); // Hash based on unique ID
    }
}
