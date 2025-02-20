using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;

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
        try
        {
            //Controleer of er al een room  met dezelfde naam in de database is
            if (!await _ctx.DbContext.Rooms.AnyAsync(x => x.Name == request.room.Name))
            {
                await _ctx.DbContext.Rooms
                        .Where(d => d.Id == request.room.Id)
                .ExecuteUpdateAsync(u => u
                            .SetProperty(p => p.Name, request.room.Name)
                        );
            }
            else
            {
                return SuccessResponse.Failed("There is already a room with the same name!!");
            }

            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }

    public async Task<SuccessResponse> CreateRoom(RoomRequest request)
    {
        try
        {
            //Controleer of er al een room met dezelfde naam in de database is
            if (!await _ctx.DbContext.Rooms.AnyAsync(x => x.Name == request.room.Name))
            {
                //Maak een nieuwe room in de database
                await _ctx.DbContext.Rooms.AddAsync(request.room);
                await _ctx.DbContext.SaveChangesAsync();

                return SuccessResponse.Success();
            }
            else
            {
                return SuccessResponse.Failed("There is already a room with the same name!!");
            }
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }

    public async Task<SuccessResponse> DeleteRoom(SmartHomeGuidRequest request)
    {
        try
        {
            bool test = await _ctx.DbContext.Devices.AnyAsync(x => x.RoomId == request.Id);

            //Controleer of er geen apparaten in de room bevinden
            if (!test)
            {
                //Verwijder room uit de database met guid
                await _ctx.DbContext.Rooms.Where(d => d.Id == request.Id).ExecuteDeleteAsync();
                return SuccessResponse.Success();
            } else
            {
                return SuccessResponse.Failed("It is not possible to delete a room that contains devices. Replace the devices to an other room!");
            }

            
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }
}
