using SmartHome.Common.Commands.Base;

namespace SmartHome.Common.Commands.Account;

public class ReadResponse(string Id, string Name) : IReadResponse
{
    public Guid Id { get; } = Guid.Parse(Id);
    public string Name { get; } = Name;
}