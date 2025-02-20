using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common.Api;
using SmartHome.Common;
using static SmartHome.Common.Api.IRoomService;

namespace SmartHome.Backend.FastEndpoints;

public class GetAllRoomsEndpoint : BasicEndpointBase<EmptySmartHomeRequest, RoomListResponse>
{
    public required IRoomService Service { get; set; }
    public override void Configure()
    {
        Get(SharedConfig.Urls.Room.GetAllRooms);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.GetAllRooms(request));
        }
        catch (Exception ex)
        {
            await SendAsync(RoomListResponse.Error(ex));
        }
    }
}

public class UpdateRoomEndpoint : BasicEndpointBase<RoomRequest, SuccessResponse>
{
    public required IRoomService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Room.UpdateRoomName);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(RoomRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.UpdateRoomName(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}

public class CreateRoomEndpoint : BasicEndpointBase<RoomRequest, GuidResponse>
{
    public required IRoomService Service { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Room.CreateRoom);
        SecureJwtEndpoint();
    }

    public override async Task HandleAsync(RoomRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.CreateRoom(request));
        }
        catch (Exception ex)
        {
            await SendAsync(GuidResponse.Error(ex));
        }
    }
}

public class DeleteRoomEndpoint : BasicEndpointBase<SmartHomeGuidRequest, SuccessResponse>
{
    public required IRoomService Service { get; set; }
    public override void Configure()
    {
        Delete(SharedConfig.Urls.Room.DeleteRoom);
        AllowAnonymous();
    }

    public override async Task HandleAsync(SmartHomeGuidRequest request, CancellationToken ct)
    {
        try
        {
            await SendAsync(await Service.DeleteRoom(request));
        }
        catch (Exception ex)
        {
            await SendAsync(SuccessResponse.Error(ex));
        }
    }
}