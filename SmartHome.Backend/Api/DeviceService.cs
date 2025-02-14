using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.Api
{
    public class DeviceService : IDeviceService
    {
        private readonly ApiContext _context;

        public DeviceService(ApiContext context)
        {
            _context = context;
        }

        public async Task<DiviceListResponse> GetDevicesByHouseId(DeviceListRequest request)
        {
            var smartUser = await _context.GetLoggedInSmartUser(request.HomeGuid);

            var result = await _context.DbContext.Devices.ToListAsync();

            if (result == null)
                return DiviceListResponse.Failed("Not Devices found in DataBase");
            else
                return new DiviceListResponse(result);
        }

        public async Task<RoomListResponse> GetRoomsByHouseId(RoomListRequest request)
        {
            var result = await _context.DbContext.Rooms.ToListAsync();

            if (result == null)
                return RoomListResponse.Failed("Not Devices found in DataBase");
            else
                return new RoomListResponse(result);
        }

        public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
        {
            var result = await _context.DbContext.Devices
                .Where(d => d.Id == request.DeviceId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.JsonObjectConfig, request.ConfigJson));

            return SuccessResponse.Success();
        }
    }
}
