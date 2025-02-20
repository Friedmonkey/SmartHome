using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.Backend.FastEndpoints;

public class GetRoutinesOfSmartHomeWithAccessEndpoint : BasicEndpointBase<SmartHomeRequest, RoutineListResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Routine.GetRoutinesOfSmartHomeWithAccess);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(SmartHomeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetRoutinesOfSmartHome(request));
        }
        catch (Exception ex)
        {
            await SendAsync(RoutineListResponse.Error(ex));
        }
    }
}

public class CreateRoutineEndpoint : BasicEndpointBase<CreateRoutineRequest, GuidResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Routine.CreateRoutine);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(CreateRoutineRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.CreateRoutine(request));
        }
        catch (Exception ex)
        {
            await SendAsync(GuidResponse.Error(ex));
        }
    }
}

public class UpdateRoutineEndpoint : BasicEndpointBase<UpdateRoutineRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.UpdateRoutine);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(UpdateRoutineRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.UpdateRoutine(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class DeleteRoutineEndpoint : BasicEndpointBase<SmartHomeGuidRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.DeleteRoutine);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(SmartHomeGuidRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.DeleteRoutine(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class CreateRoutineActionEndpoint : BasicEndpointBase<CreateActionRequest, GuidResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Delete(SharedConfig.Urls.Routine.CreateDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(CreateActionRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.CreateDeviceAction(request));
        }
        catch (Exception ex)
        {
            await SendAsync(GuidResponse.Error(ex));
        }
    }
}

public class UpdateRoutineActionEndpoint : BasicEndpointBase<UpdateActionRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.UpdateDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(UpdateActionRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.UpdateDeviceAction(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class DeleteRoutineActionEndpoint : BasicEndpointBase<Guid, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.DeleteDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(Guid request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.DeleteDeviceAction(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}


