using SmartHome.Shared.Commands.User;
using SmartHome.Shared.Models.Enums;

namespace SmartHome.Backend.Features.User;

public class Create(SmartHomeDbContext _SmartHomeDbContext)
    : CreateEndpointBase<CreateCommand, CreateResponse, Entity.User, CreateMapper, CreateCommandValidator>(_SmartHomeDbContext)
{
    protected override UserRole[] RoleNames => [UserRole.Admin];

    protected override void ConfigureEndpoint()
    {
        Post("ser/create");
        Roles(RoleNames.Select(x => x.ToString()).ToArray());
    }
}

public class CreateMapper : Mapper<CreateCommand, CreateResponse, Entity.User>
{
    public override Entity.User ToEntity(CreateCommand r)
    {
        return new Entity.User
        {
            Name = r.Name
        };
    }

    public override CreateResponse FromEntity(Entity.User e)
    {
        return new CreateResponse(e.Id);
    }
}