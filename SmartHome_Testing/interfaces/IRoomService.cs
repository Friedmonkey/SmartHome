using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome_Testing.interfaces
{
    public class RoomServiceTest : IRoomService
    {
        public List<Room> TestRooms = new List<Room>();

        public async Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request)
        {
            return new RoomListResponse(TestRooms);
        }

        public Task<SuccessResponse> UpdateRoomName(RoomRequest request)
        {
            return null;
        }

        public Task<GuidResponse> CreateRoom(RoomRequest request)
        {
            return null;
        }

        public Task<SuccessResponse> DeleteRoom(SmartHomeGuidRequest request)
        {
            return null;
        }
    }
}
