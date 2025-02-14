using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SmartHome.Common.Models.Enums;
using SmartHome.Database.Auth;

namespace SmartHome.Common.Models.Entities;

public class SmartUserModel : Entity
{
    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public Guid SmartHomeId { get; set; }

    [Required]
    public UserRole Role { get; set; }

    public AuthAccount? Account { get; set; }
    public SmartHomeModel? SmartHome { get; set; }
}
