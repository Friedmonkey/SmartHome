using SmartHome.Common.Commands.Account;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Features.Account;

public class Update(SmartHomeDbContext _SmartHomeDbContext)
    : UpdateEndpointBase<UpdateCommand, UpdateResponse, Entity.Account, UpdateMapper, UpdateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];
    protected override void ConfigureEndpoint()
    {
        Post("user/update");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class UpdateMapper : Mapper<UpdateCommand, UpdateResponse, Entity.Account>
{
    public override Entity.Account UpdateEntity(UpdateCommand r, Entity.Account e)
    {
        e.Name = r.Name;
        return e;
    }

    public override UpdateResponse FromEntity(Entity.Account e)
    {
        return new UpdateResponse();
    }
}