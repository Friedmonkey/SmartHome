using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
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
            var result = await _context.DbContext.Devices.ToListAsync();

            if (result == null)
                return DiviceListResponse.Failed("Not Devices found in DataBase");
            else
                return new DiviceListResponse(result);
        }

        public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
        {
            var result = await _context.DbContext.Devices.ToListAsync();

            return SuccessResponse.Success();
        }
    }
}
