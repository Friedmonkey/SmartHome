using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record RoomListResponse(List<Room> Rooms) : Response<RoomListResponse>;
public record RoomRequest(Room room) : SmartHomeRequest;

public interface IRoomService
{
    Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request);

    Task<SuccessResponse> UpdateRoomName(RoomRequest request);

    Task<GuidResponse> CreateRoom(RoomRequest request);

    Task<SuccessResponse> DeleteRoom(SmartHomeGuidRequest request);
}