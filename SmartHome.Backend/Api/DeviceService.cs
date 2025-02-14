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

        public async Task<DiviceListResponse> GetDevicesByHomeId(DeviceListRequest request)
        {
            try
            {
                //Haal het account uit de DataBaase
                //var smartUser = await _context.GetLoggedInSmartUser(request.HomeGuid);
                var smartUser = await _context.GetSmartUser(Guid.Parse("08dd4d36-b85e-4f89-8568-31d7498c60ec"), Guid.Parse("d68b031b-a81c-43aa-b589-c4a0da0bb442"));

                //Haal de device toegang op bij het ingelogde account
                List<DeviceAccess> devicesAccessList = new List<DeviceAccess>();
                devicesAccessList = await _context.DbContext.DeviceAccesses.Where(a => a.SmartUserId == smartUser.Id).ToListAsync();

                //Haal de rooms uit de database die in het huis staan
                List<Room> roomsList = new List<Room>();
                roomsList = await _context.DbContext.Rooms.Where(r => r.SmartHomeId == request.HomeGuid).ToListAsync();

                //Haal de apparaten op en filter devices met de roomids die in het huis staan
                List<Device> deviceList = new List<Device>();
                deviceList = await _context.DbContext.Devices.ToListAsync();

                //Filter Devices op room die bij een home hoort
                deviceList = deviceList.Where(d => roomsList.Select(r => r.Id).ToArray().Contains(d.RoomId)).ToList();

                //Filer de devices waar het account toegang tot heeft
                deviceList = deviceList.Where(d => devicesAccessList.Select(a => a.DeviceId).ToArray().Contains(d.Id)).ToList();

                return new DiviceListResponse(deviceList);
            } catch(Exception ex)
            {
                return DiviceListResponse.Failed(ex.Message);
            }
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
