using SmartHome.Shared.Commands.User;
using SmartHome.Backend.Features;
using SmartHome.Shared.Models.Enums;

namespace SmartHome.Backend.Features.User;

public class Delete(SmartHomeDbContext _SmartHomeDbContext)
    : DeleteEndpointBase<DeleteCommand, Entity.User, DeleteCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];

    protected override void ConfigureEndpoint()
    {
        Post("user/delete");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}