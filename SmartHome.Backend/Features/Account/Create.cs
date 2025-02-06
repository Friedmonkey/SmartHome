using SmartHome.Common.Commands.Account;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Backend.Features.Account;

public class Create(SmartHomeDbContext _SmartHomeDbContext)
    : CreateEndpointBase<CreateCommand, CreateResponse, Entity.Account, CreateMapper, CreateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];

    protected override void ConfigureEndpoint()
    {
        Post("ser/create");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class CreateMapper : Mapper<CreateCommand, CreateResponse, Entity.Account>
{
    public override Entity.Account ToEntity(CreateCommand r)
    {
        return new Entity.Account
        {
            Name = r.Name
        };
    }

    public override CreateResponse FromEntity(Entity.Account e)
    {
        return new CreateResponse(e.Id);
    }
}