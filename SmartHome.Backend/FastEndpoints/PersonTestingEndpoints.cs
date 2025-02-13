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
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(AddPersonRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.AddPerson(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}
public class GetPersonByAgeEndpoint : BasicEndpointBase<GetPersonByAgeRequest, PersonResponse>
{
    public required IPersonTestingService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Person.GetByAgeUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(GetPersonByAgeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetPersonByAge(request));
        }
        catch (Exception ex)
        {
            await SendAsync(PersonResponse.Error(ex));
        }
    }
}

public class GetPersonByNameEndpoint : BasicEndpointBase<GetPersonByNameRequest, PersonResponse>
{
    public required IPersonTestingService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Person.GetByNameUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(GetPersonByNameRequest request, CancellationToken ct)
    {
        try
        { 
            await SendAsync(await Service.GetPersonByName(request));
        }
        catch (Exception ex)
        {
            await SendAsync(PersonResponse.Error(ex));
        }
    }
}


