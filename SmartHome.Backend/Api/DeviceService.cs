using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.Api;

public class DeviceService : IDeviceService
{
    private readonly ApiContext _context;

    public DeviceService(ApiContext context)
    {
        _context = context;
    }

    public async Task<DeviceListResponse> GetDevicesWithAccess(DeviceListRequest request)
    {
        try
        {
            //Haal het account uit de DataBaase
            var smartUser = await _context.GetLoggedInSmartUser(request.HomeGuid);

            //Haal de device toegang op bij het ingelogde account
            List<DeviceAccess> devicesAccessList = new List<DeviceAccess>();
            devicesAccessList = await _context.DbContext.DeviceAccesses.Where(a => a.SmartUserId == smartUser.Id).ToListAsync();

            //Haal de rooms uit de database die in het huis staan
            List<Room> roomsList = new List<Room>();
            roomsList = await _context.DbContext.Rooms.Where(r => r.SmartHomeId == request.HomeGuid).ToListAsync();

            //Haal de apparaten op en filter devices met de roomids die in het huis staan
            List<Device> deviceList = new List<SmartHome.Common.Models.Entities.Device>();
            deviceList = await _context.DbContext.Devices.ToListAsync();

            //Filter Devices op room die bij een home hoort
            deviceList = deviceList.Where(d => roomsList.Select(r => r.Id).ToArray().Contains(d.RoomId)).ToList();

            //Filer de devices waar het account toegang tot heeft
            deviceList = deviceList.Where(d => devicesAccessList.Select(a => a.DeviceId).ToArray().Contains(d.Id)).ToList();

            return new DeviceListResponse(deviceList);
        }
        catch (Exception ex)
        {
            return DeviceListResponse.Failed(ex.Message);
        }
    }

    public async Task<DeviceListResponse> GetAllDevices(AllDeviceListRequest request)
    {
        try
        {
            //Haal het account uit de DataBaase
            //var smartUser = await _context.GetLoggedInSmartUser(request.HomeGuid);
            //var smartUser = await _context.GetSmartUser(Guid.Parse("08dd4d36-b85e-4f89-8568-31d7498c60ec"), Guid.Parse("d68b031b-a81c-43aa-b589-c4a0da0bb442"));

            //Haal de rooms uit de database die in het huis staan
            List<Room> roomsList = new List<Room>();
            roomsList = await _context.DbContext.Rooms.Where(r => r.SmartHomeId == request.HomeGuid).ToListAsync();

            //Haal de apparaten op en filter devices met de roomids die in het huis staan
            List<Device> deviceList = new List<Device>();
            deviceList = await _context.DbContext.Devices.ToListAsync();

            //Filter Devices op room die bij een home hoort
            deviceList = deviceList.Where(d => roomsList.Select(r => r.Id).ToArray().Contains(d.RoomId)).ToList();

            return new DeviceListResponse(deviceList);
        }
        catch (Exception ex)
        {
            return DeviceListResponse.Failed(ex.Message);
        }
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        try
        {
            foreach (Device device in request.devices)
            {
                //Update de device list met de propperties naar de DataBase

                await _context.DbContext.Devices
                    .Where(d => d.Id == device.Id)
                    .ExecuteUpdateAsync(u => u
                        .SetProperty(p => p.Name, device.Name)
                        .SetProperty(p => p.JsonObjectConfig, device.JsonObjectConfig)
                        .SetProperty(p => p.RoomId, device.Room.Id)
                        .SetProperty(p => p.Type, device.Type)
                    );
            }

            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }

    public async Task<SuccessResponse> UpdateDevice(UpdateDeviceRequest request)
    {
        try
        {
            //Controleer of er al een device met dezelfde naam in de database is
            if (!await _context.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
            {
                await _context.DbContext.Devices
                        .Where(d => d.Id == request.device.Id)
                .ExecuteUpdateAsync(u => u
                            .SetProperty(p => p.Name, request.device.Name)
                            .SetProperty(p => p.JsonObjectConfig, request.device.JsonObjectConfig)
                            .SetProperty(p => p.RoomId, request.device.RoomId)
                            .SetProperty(p => p.Type, request.device.Type)
                        );
            }
            else
            {
                return SuccessResponse.Failed("There is already a device with the same name!!");
            }

            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }

    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        try
        {
            //Verwijder device uit de database met guid
            await _context.DbContext.Devices.Where(d => d.Id == request.DeviceGuid).ExecuteDeleteAsync();

            //Verwidjer alle user acces met het device id
            await _context.DbContext.DeviceAccesses.Where(d => d.DeviceId == request.DeviceGuid).ExecuteDeleteAsync();

            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.Failed(ex.Message);
        }
    }

    public async Task<DeviceCreateResponse> CreateDevice(CreateDeviceRequest request)
    {
        try
        {
            //Controleer of er al een device met dezelfde naam in de database is
            if (!await _context.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
            {
                //Maak een nieuwe device in de database
                await _context.DbContext.Devices.AddAsync(request.device);
                await _context.DbContext.SaveChangesAsync();

                //Haal alle smartusers op in de home
                var SmartUserIdList = await _context.DbContext.SmartUsers.Where(u => u.SmartHomeId == request.SmartHomeId).Select(u => u.Id).ToListAsync();

                //Geef alle users in de home acces voor het nieuwe apparaat
                foreach(Guid SmartUserId in SmartUserIdList)
                {
                    DeviceAccess deviceAccess = new DeviceAccess();
                    deviceAccess.DeviceId = request.device.Id;
                    deviceAccess.SmartUserId = SmartUserId;

                    await _context.DbContext.DeviceAccesses.AddAsync(deviceAccess);
                }

                await _context.DbContext.SaveChangesAsync();

                return new DeviceCreateResponse(request.device.Id);
            }
            else
            {
                return DeviceCreateResponse.Failed("There is already a device with the same name!!");
            }
        }
        catch (Exception ex)
        {
            return DeviceCreateResponse.Failed(ex.Message);
        }
    }


    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        var result = await _context.DbContext.Devices
            .Where(d => d.Id == request.DeviceId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.JsonObjectConfig, request.ConfigJson));

        return SuccessResponse.Success();
    }
}
