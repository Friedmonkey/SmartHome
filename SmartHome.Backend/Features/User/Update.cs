using SmartHome.Shared.Commands.User;
using SmartHome.Shared.Models.Enums;

namespace SmartHome.Backend.Features.User;

public class Update(SmartHomeDbContext _SmartHomeDbContext)
    : UpdateEndpointBase<UpdateCommand, UpdateResponse, Entity.User, UpdateMapper, UpdateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];
    protected override void ConfigureEndpoint()
    {
        Post("user/update");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class UpdateMapper : Mapper<UpdateCommand, UpdateResponse, Entity.User>
{
    public override Entity.User UpdateEntity(UpdateCommand r, Entity.User e)
    {
        e.Name = r.Name;
        return e;
    }

    public override UpdateResponse FromEntity(Entity.User e)
    {
        return new UpdateResponse();
    }
}