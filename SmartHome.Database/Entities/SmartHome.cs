namespace SmartHome.Database.Entities;
public class SmartHome
    : Entity
{
    public string? Name { get; set; }
    public string? SSID { get; set; }
    public string? SSPassword { get; set; }
    public List<Room>? Rooms { get; set; }
}
