using SmartHome.Common.Commands.Account;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Features.Account;

public class Create(SmartHomeContext _SmartHomeDbContext)
    : CreateEndpointBase<CreateCommand, CreateResponse, Common.Models.Entities.Account, CreateMapper, CreateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];

    protected override void ConfigureEndpoint()
    {
        Post("User/create");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class CreateMapper : Mapper<CreateCommand, CreateResponse, Common.Models.Entities.Account>
{
    public override Common.Models.Entities.Account ToEntity(CreateCommand r)
    {
        return new Entity.Account
        {
            Name = r.Name
        };
    }

    //public override CreateResponse FromEntity(SmartHome.Common.Models.Entities.Account e)
    //{
    //    return new CreateResponse(e.Guid);
    //}
}