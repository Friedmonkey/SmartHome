using System.Text.Json.Nodes;

namespace SmartHome.Database.Entities;
public class DeviceAction
    : Entity
{
    public string? Name { get; set; }
    public JsonObject? Config { get; set; }
    public Guid DeviceId { get; set; }
    public Guid RoutineId { get; set; }
    public Device? Device { get; set; }
    public Routine Routine { get; set; }
}
