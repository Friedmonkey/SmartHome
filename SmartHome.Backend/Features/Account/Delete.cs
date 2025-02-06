using SmartHome.Backend.Features;
using SmartHome.Common.Models.Enums;
using SmartHome.Common.Commands.Account;

namespace SmartHome.Backend.Features.User;

public class Delete(SmartHomeDbContext _SmartHomeDbContext)
    : DeleteEndpointBase<DeleteCommand, Entity.Account, DeleteCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];

    protected override void ConfigureEndpoint()
    {
        Post("user/delete");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}