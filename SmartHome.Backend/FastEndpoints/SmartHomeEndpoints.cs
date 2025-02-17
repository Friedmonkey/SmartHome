using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common.Api;
using SmartHome.Common;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.Backend.FastEndpoints;

public class CreateSmartHomeEndpoint : BasicEndpointBase<CreateSmartHomeRequest, GuidResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartHome.CreateSmartHomeUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(CreateSmartHomeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.CreateSmartHome(request));
        }
        catch (Exception ex)
        {
            await SendAsync(GuidResponse.Error(ex));
        }
    }
}

public class InviteToSmartHomeEndpoint : BasicEndpointBase<InviteRequest, SuccessResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartHome.InviteToSmartHomeUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(InviteRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.InviteToSmartHome(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class AcceptSmartHomeInviteEndpoint : BasicEndpointBase<SmartHomeRequest, SuccessResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.SmartHome.AcceptInviteToSmartHomeUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(SmartHomeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.AcceptSmartHomeInvite(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class GetJoinedSmartHomesEndpoint : BasicEndpointBase<EmptyRequest, SmartHomeListResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.SmartHome.GetJoinedUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(EmptyRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetJoinedSmartHomes(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SmartHomeListResponse.Error(ex));
        }
    }
}
public class GetSmartHomeInvitesEndpoint : BasicEndpointBase<EmptyRequest, SmartHomeListResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.SmartHome.GetInvitesUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(EmptyRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetSmartHomeInvites(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SmartHomeListResponse.Error(ex));
        }
    }
}

public class GetSmartHomeByIdEndpoint : BasicEndpointBase<GuidRequest, SmartHomeResponse>
{
    public required ISmartHomeService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.SmartHome.GetByIDUrl);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(GuidRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetSmartHomeById(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SmartHomeResponse.Error(ex));
        }
    }
}