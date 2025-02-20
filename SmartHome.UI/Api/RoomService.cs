using SmartHome.Common.Api;
using SmartHome.Common;

namespace SmartHome.UI.Api;

public class RoomService : IRoomService
{
    private readonly ApiService _api;

    public RoomService(ApiService api)
    {
        this._api = api;
    }

    public async Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request)
    {
        return await _api.Get<RoomListResponse>(SharedConfig.Urls.Room.GetAllRooms, request);
    }

    public async Task<SuccessResponse> UpdateRoomName(RoomRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Room.UpdateRoomName, request);
    }

    public async Task<GuidResponse> CreateRoom(RoomRequest request)
    {
        return await _api.Post<GuidResponse>(SharedConfig.Urls.Room.CreateRoom, request);
    }

    public async Task<SuccessResponse> DeleteRoom(SmartHomeGuidRequest request)
    {
        return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Room.DeleteRoom, request);
    }
}
