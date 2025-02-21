using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Backend.Api;

public class RoomService : IRoomService
{
    private readonly ApiContext _ctx;

    public RoomService(ApiContext context)
    {
        _ctx = context;
    }

    public async Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request)
    {
        var result = await _ctx.DbContext.Rooms.Where(r => r.SmartHomeId == request.smartHome).ToListAsync();

        return new RoomListResponse(result);
    }

    public async Task<SuccessResponse> UpdateRoomName(RoomRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);

        await _ctx.Room.EnforceRoomNameUnique(request.smartHome, request.room?.Name);
        //Controleer of er al een room  met dezelfde naam in de database is
        await _ctx.DbContext.Rooms
            .Where(d => d.Id == request.room.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Name, request.room.Name)
            );

        return SuccessResponse.Success();
    }

    public async Task<GuidResponse> CreateRoom(RoomRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Room.EnforceRoomNameUnique(request.smartHome, request.room.Name);

        Room newRoom = new Room()
        {
            Name = request.room.Name,
            SmartHomeId = request.smartHome,
        };
        var result = await _ctx.DbContext.Rooms.AddAsync(newRoom);
        await _ctx.DbContext.SaveChangesAsync();

        return new GuidResponse(result.Entity.Id);
    }

    public async Task<SuccessResponse> DeleteRoom(SmartHomeGuidRequest request)
    {
        bool test = await _ctx.DbContext.Devices.AnyAsync(x => x.RoomId == request.Id);

        //Controleer of er geen apparaten in de room bevinden
        if (!test)
        {
            //Verwijder room uit de database met guid
            await _ctx.DbContext.Rooms.Where(d => d.Id == request.Id).ExecuteDeleteAsync();
            return SuccessResponse.Success();
        }
        else
        {
            return SuccessResponse.Failed("It is not possible to delete a room that contains devices. Replace the devices to an other room!");
        }
    }
}
