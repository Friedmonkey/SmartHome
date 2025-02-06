using System.Text.Json.Nodes;

namespace SmartHome.Database.Entities;

public class DeviceType
    : Entity
{
    public string? Name { get; set; }
    public JsonObject DefaultConfig { get; set; }
}