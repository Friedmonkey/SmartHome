using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.ISmartUserService;

namespace SmartHome.Backend.FastEndpoints;

public class CreateSmartUserEndpoint : BasicEndpointBase<CreateRequest, SuccessResponse>
{
    public required ISmartUserService SmartUserService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartUser.AddSmartUserUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateRequest request, CancellationToken ct)
    {
        await SendAsync(await SmartUserService.Create(request));
    }
}

public class GetSmartUsersOfSmartUser : BasicEndpointBase<RequestByGuid, SmartUserResponse>
{
    public required ISmartUserService SmartUserService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartUser.AddSmartUserUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RequestByGuid request, CancellationToken ct)
    {
        await SendAsync(await SmartUserService.GetSmartUsersOfAccount(request));
    }
}

public class UpdateSmartUserEndpoint : BasicEndpointBase<UpdateRequest, SuccessResponse>
{
    public required ISmartUserService SmartUserService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartUser.UpdateSmartHomeUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateRequest request, CancellationToken ct)
    {
        await SendAsync(await SmartUserService.Update(request));
    }
}
public class DeleteSmartUserEndpoint : BasicEndpointBase<RequestByGuid, SuccessResponse>
{
    public required ISmartUserService SmartUserService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartUser.DeleteSmartUserUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RequestByGuid request, CancellationToken ct)
    {
        await SendAsync(await SmartUserService.Delete(request));
    }
}