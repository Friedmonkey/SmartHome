namespace SmartHome.Database.Entities;

public class SmartUser
    : Entity
{
    public Guid RoleId { get; set; }
    public Guid AccountId { get; set; }
    public Guid SmartHomeId { get; set; }
    public Role Role { get; set; }
    public Account Account { get; set; }
    public SmartHome SmartHome { get; set; }
}
