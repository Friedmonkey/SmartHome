using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartHome.Common.Models.Enums;
using SmartHome.Database.Auth;

namespace SmartHome.Common.Models.Entities;

public class SmartUser : Entity
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    [Required]
    public UserRole RoleId { get; set; }

    public AuthAccount? Account { get; set; }
    public SmartHome? SmartHome { get; set; }
}
