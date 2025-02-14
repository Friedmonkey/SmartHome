using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.INewDeviceService;

namespace SmartHome.Backend.Api
{
    public class DeviceService : INewDeviceService
    {
        private ApiContext _ctx;
        public DeviceService(ApiContext apiContext)
        {
            _ctx = apiContext;
        }
        public async Task<GuidResponse> CreateDevice(CreateDeviceRequest request)
        {   //only admins can create devices
            await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);

            Device device = new Device()
            { 
                Name = request.deviceName,
                 = request.deviceType
            };
            await _ctx.DbContext.Devices.AddAsync(device);
            throw new NotImplementedException();
        }

        public async Task<DeviceListResponse> GetRoomsAndDevicesForUser(SmartHomeRequest request)
        {
            var smartUser = await _ctx.GetLoggedInSmartUser(request.smartHome);
            throw new NotImplementedException();
        }
    }
}
