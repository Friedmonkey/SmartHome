using SmartHome.Common.Commands.Base;

namespace SmartHome.Common.Commands.Account;

public class ReadResponse(
    Guid Id,
    string Name)
    : IReadResponse
{
    public Guid Id { get; } = Id;
    public string Name { get; } = Name;
}