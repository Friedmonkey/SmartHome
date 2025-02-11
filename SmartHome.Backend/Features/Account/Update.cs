using SmartHome.Common.Commands.Account;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Features.Account;

public class Update(SmartHomeContext _SmartHomeDbContext)
    : UpdateEndpointBase<UpdateCommand, UpdateResponse, Common.Models.Entities.Account, UpdateMapper, UpdateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];
    protected override void ConfigureEndpoint()
    {
        Post("user/update");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class UpdateMapper : Mapper<UpdateCommand, UpdateResponse, Common.Models.Entities.Account>
{
    public override Common.Models.Entities.Account UpdateEntity(UpdateCommand r, Common.Models.Entities.Account e)
    {
        e.Name = r.Name;
        return e;
    }

    public override UpdateResponse FromEntity(Common.Models.Entities.Account e)
    {
        return new UpdateResponse();
    }
}