using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IRoomService;

namespace SmartHome.Backend.Api
{
    public class RoomService : IRoomService
    {
        private readonly ApiContext _context;

        public RoomService(ApiContext context)
        {
            _context = context;
        }

        public async Task<RoomListResponse> GetAllRooms(RoomListRequest request)
        {
            var result = await _context.DbContext.Rooms.ToListAsync();

            if (result == null)
                return RoomListResponse.Failed("No Rooms found in DataBase");
            else
                return new RoomListResponse(result);
        }

        public async Task<SuccessResponse> UpdateRoomName(UpdateRoomRequest request)
        {
            try
            {
                //Controleer of er al een room  met dezelfde naam in de database is
                if (!await _context.DbContext.Rooms.AnyAsync(x => x.Name == request.room.Name))
                {
                    await _context.DbContext.Rooms
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

        public async Task<SuccessResponse> CreateRoom(CreateRoomRequest request)
        {
            try
            {
                //Controleer of er al een room met dezelfde naam in de database is
                if (!await _context.DbContext.Rooms.AnyAsync(x => x.Name == request.room.Name))
                {
                    //Maak een nieuwe room in de database
                    await _context.DbContext.Rooms.AddAsync(request.room);
                    await _context.DbContext.SaveChangesAsync();

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

        public async Task<SuccessResponse> DeleteRoom(DeleteRoomRequest request)
        {
            try
            {
                //Controleer of er geen apparaten in de room bevinden
                if (!await _context.DbContext.Devices.AnyAsync(x => x.RoomId == request.RoomGuid))
                {
                    //Verwijder room uit de database met guid
                    await _context.DbContext.Rooms.Where(d => d.Id == request.RoomGuid).ExecuteDeleteAsync();
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
}
