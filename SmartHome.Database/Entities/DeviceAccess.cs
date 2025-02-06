namespace SmartHome.Database.Entities;

public class DeviceAccess
    : Entity
{
    public Guid DeviceId { get; set; }
    public Guid SmartUserId { get; set; }
    public Device? Device { get; set; }
    public SmartHome? SmartHome { get; set; }
}
