using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record RoomListResponse(List<Room> Rooms) : Response<RoomListResponse>;

public interface IRoomService
{
    public record RoomListRequest(Guid HomeGuid);
    Task<RoomListResponse> GetAllRooms(RoomListRequest request);

    public record UpdateRoomRequest(Room room);
    Task<SuccessResponse> UpdateRoomName(UpdateRoomRequest request);

    public record CreateRoomRequest(Room room);
    Task<SuccessResponse> CreateRoom(CreateRoomRequest request);

    public record DeleteRoomRequest(Guid RoomGuid);
    Task<SuccessResponse> DeleteRoom(DeleteRoomRequest request);
}