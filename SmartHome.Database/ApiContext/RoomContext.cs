using Microsoft.EntityFrameworkCore;
using SmartHome.Common;

namespace SmartHome.Database.ApiContext;

public class RoomContext
{
    private readonly SmartHomeContext _dbContext;

    public RoomContext(SmartHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task EnforceRoomNameUnique(Guid smartHomeId, string? roomName)
    {
        if (string.IsNullOrEmpty(roomName))
            throw new ApiError("Room name cannot be empty!");

        bool alreadyExists = await _dbContext.Rooms.AnyAsync(x => x.Name == roomName);

        if (alreadyExists)
            throw new ApiError("There is already a room with the same name!!");
    }
}
