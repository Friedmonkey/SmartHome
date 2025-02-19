using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common.Api;
using SmartHome.Common;
using static SmartHome.Common.Api.IRoomService;

namespace SmartHome.Backend.FastEndpoints
{
    public class GetAllRoomsEndpoint : BasicEndpointBase<RoomListRequest, RoomListResponse>
    {
        public required IRoomService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Room.GetAllRooms);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(RoomListRequest request, CancellationToken ct)
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

    public class UpdateRoomEndpoint : BasicEndpointBase<UpdateRoomRequest, SuccessResponse>
    {
        public required IRoomService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Room.UpdateRoom);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(UpdateRoomRequest request, CancellationToken ct)
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

    //public class CreateRoomEndpoint : BasicEndpointBase<CreateRoomRequest, SuccessResponse>
    //{
    //    public required IRoomService Service { get; set; }
    //    public override void Configure()
    //    {
    //        Post(SharedConfig.Urls.Room.CreateRoom);
    //        SecureJwtEndpoint();
    //    }

    //    public override async Task HandleAsync(CreateRoomRequest request, CancellationToken ct)
    //    {
    //        try
    //        {
    //            await SendAsync(await Service.CreateRoom(request));
    //        }
    //        catch (Exception ex)
    //        {
    //            await SendAsync(SuccessResponse.Error(ex));
    //        }
    //    }
    //}

    public class DeleteRoomEndpoint : BasicEndpointBase<DeleteRoomRequest, SuccessResponse>
    {
        public required IRoomService Service { get; set; }
        public override void Configure()
        {
            Delete(SharedConfig.Urls.Room.DeleteRoom);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(DeleteRoomRequest request, CancellationToken ct)
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
}