namespace SmartHome.Database.Entities;

public class Account
    : Entity
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? PasswordHashed { get; set; }
    public string? SecurityStamp { get; set; }
}