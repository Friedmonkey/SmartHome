namespace SmartHome.Database.Entities;
public class Routine
    : Entity
{
    public string? Name { get; set; }
    public Guid SmartHomeId { get; set; }
    public DateTime Start { get; set; }
    public Byte RepeatDays { get; set; }
    public SmartHome? SmartHome { get; set; }
}
