namespace SmartHome.Database.Entities;

public class Room
    : Entity
{
    public string? Name { get; set; }
    public Guid SmartHomeId { get; set; }
    public SmartHome SmartHome { get; set; }
    public List<Device> Devices { get; set; }
}
