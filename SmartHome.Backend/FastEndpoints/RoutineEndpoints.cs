using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.Backend.FastEndpoints;

public class GetAllRoutinesEndpoint : BasicEndpointBase<EmptySmartHomeRequest, RoutineListResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Routine.GetAllRoutines);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetAllRoutines(request));
        }
        catch (Exception ex)
        {
            await SendAsync(RoutineListResponse.Error(ex));
        }
    }
}

public class CreateRoutineEndpoint : BasicEndpointBase<RoutineRequest, GuidResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.CreateRoutine);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(RoutineRequest request, CancellationToken ct)
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

public class UpdateRoutineEndpoint : BasicEndpointBase<RoutineRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.UpdateRoutine);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(RoutineRequest request, CancellationToken ct)
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
        Delete(SharedConfig.Urls.Routine.DeleteRoutine);
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

public class CreateRoutineActionEndpoint : BasicEndpointBase<DeviceActionRequest, GuidResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.CreateDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(DeviceActionRequest request, CancellationToken ct)
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

public class UpdateRoutineActionEndpoint : BasicEndpointBase<DeviceActionRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Routine.UpdateDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(DeviceActionRequest request, CancellationToken ct)
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

public class DeleteRoutineActionEndpoint : BasicEndpointBase<SmartHomeGuidRequest, SuccessResponse>
{
    public required IRoutineService Service { get; set; }
    public override void Configure()
    {
        Delete(SharedConfig.Urls.Routine.DeleteDeviceAction);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(SmartHomeGuidRequest request, CancellationToken ct)
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


