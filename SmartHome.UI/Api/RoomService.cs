using SmartHome.Common.Api;
using SmartHome.Common;
using static SmartHome.Common.Api.IRoomService;

namespace SmartHome.UI.Api
{
    public class RoomService : IRoomService
    {
        private readonly ApiService _api;

        public RoomService(ApiService api)
        {
            this._api = api;
        }

        public async Task<RoomListResponse> GetAllRooms(RoomListRequest request)
        {
            return await _api.Get<RoomListResponse>(SharedConfig.Urls.Room.GetAllRooms, request);
        }

        public async Task<SuccessResponse> UpdateRoomName(UpdateRoomRequest request)
        {
            return await _api.Get<SuccessResponse>(SharedConfig.Urls.Room.UpdateRoom, request);
        }

        public async Task<SuccessResponse> CreateRoom(CreateRoomRequest request)
        {
            return await _api.Get<SuccessResponse>(SharedConfig.Urls.Room.CreateRoom, request);
        }

        public async Task<SuccessResponse> DeleteRoom(DeleteRoomRequest request)
        {
            return await _api.Get<SuccessResponse>(SharedConfig.Urls.Room.DeleteRoom, request);
        }
    }
}
