using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Commands.Account;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Features.User;
public class Read(SmartHomeContext _SmartHomeDbContext) : EndpointWithoutRequest<Ok<List<ReadResponse>>>
{
    public override void Configure()
    {
        Get("user");
        UserRole[] RoleNames = [UserRole.Admin];
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }

    public override async Task<Ok<List<ReadResponse>>> ExecuteAsync(CancellationToken ct)
    {

        var customers = await _SmartHomeDbContext
            .Accounts
            .Select(x => new ReadResponse(x.Id, x.Name!))
            .ToListAsync(ct);

        return Ok(customers);
    }
}