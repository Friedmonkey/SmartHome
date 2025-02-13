using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Common.Models.Entities;

public class SmartUser : Entity
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    [Required]
    public UserRole RoleId { get; set; }

    public Account? Account { get; set; }
    public SmartHome? SmartHome { get; set; }
}
