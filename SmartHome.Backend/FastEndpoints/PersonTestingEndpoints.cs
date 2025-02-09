using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IPersonTestingService;


namespace SmartHome.Backend.FastEndpoints;

public class AddPersonEndpoint : BasicEndpointBase<AddPersonRequest, SuccessResponse>
{
    public required IPersonTestingService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Person.AddPersonUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddPersonRequest request, CancellationToken ct)
    {
        await SendAsync(await Service.AddPerson(request));
    }
}

public class GetPersonByAgeEndpoint : BasicEndpointBase<GetPersonByAgeRequest, PersonResponse>
{
    public required IPersonTestingService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Person.GetByAgeUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPersonByAgeRequest request, CancellationToken ct)
    {
        await SendAsync(await Service.GetPersonByAge(request));
    }
}

public class GetPersonByNameEndpoint : BasicEndpointBase<GetPersonByNameRequest, PersonResponse>
{
    public required IPersonTestingService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Person.GetByNameUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetPersonByNameRequest request, CancellationToken ct)
    {
        await SendAsync(await Service.GetPersonByName(request));
    }
}


