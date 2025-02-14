using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SmartHome.Common.Models.Entities;

public class DeviceAccess : Entity
{
    [Required]
    public Guid DeviceId { get; set; }

    [Required]
    public Guid SmartUserId { get; set; }

    public Device Device { get; set; }
    public SmartUserModel SmartUser { get; set; }
}
