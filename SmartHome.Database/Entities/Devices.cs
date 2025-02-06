using System.Text.Json.Nodes;

namespace SmartHome.Database.Entities;
public class Device
    : Entity
{
    public string? Name { get; set; }
    public Guid DeviceTypeId { get; set; }
    public JsonObject Config { get; set; }
    public Guid RoomId { get; set; }
    public Room? Room { get; set; }
    public DeviceType? DeviceType { get; set; }
}
