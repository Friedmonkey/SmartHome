namespace SmartHome.Shared.Commands.User;

public class ReadResponse(
    Guid Id,
    string Name)
    : IReadResponse
{
    public Guid Id { get; } = Id;
    public string Name { get; } = Name;
}