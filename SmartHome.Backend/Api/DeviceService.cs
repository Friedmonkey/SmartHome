using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.Api;

public class DeviceService : IDeviceService
{
    private readonly ApiContext _ctx;

    public DeviceService(ApiContext context)
    {
        _ctx = context;
    }

    public async Task<DeviceListResponse> GetDevicesWithAccess(SmartHomeRequest request)
    {
        var smartUser = await _ctx.GetLoggedInSmartUser(request.smartHome);

        // Get all necessary data in a single step
        var deviceList = await _ctx.DbContext.Devices
            .Where(d =>
                _ctx.DbContext.Rooms
                    .Where(r => r.SmartHomeId == request.smartHome)
                    .Select(r => r.Id)
                    .Contains(d.RoomId) && // Device belongs to the Smart Home
                _ctx.DbContext.DeviceAccesses
                    .Where(a => a.SmartUserId == smartUser.Id)
                    .Select(a => a.DeviceId)
                    .Contains(d.Id) // User has access to the device
            ).ToListAsync();

        return new DeviceListResponse(deviceList);
    }


    //public async Task<DeviceListResponse> GetDevicesWithAccess(SmartHomeRequest request)
    //{
    //    var smartUser = await _ctx.GetLoggedInSmartUser(request.smartHome);

    //    //Haal de device toegang op bij het ingelogde account
    //    List<DeviceAccess> devicesAccessList = new List<DeviceAccess>();
    //    devicesAccessList = await _ctx.DbContext.DeviceAccesses.Where(a => a.SmartUserId == smartUser.Id).ToListAsync();

    //    //Haal de rooms uit de database die in het huis staan
    //    List<Room> roomsList = new List<Room>();
    //    roomsList = await _ctx.DbContext.Rooms.Where(r => r.SmartHomeId == request.smartHome).ToListAsync();

    //    //Haal de apparaten op en filter devices met de roomids die in het huis staan
    //    List<Device> deviceList = new List<Device>();
    //    deviceList = await _ctx.DbContext.Devices.ToListAsync();

    //    //Filter Devices op room die bij een home hoort
    //    deviceList = deviceList.Where(d => roomsList.Select(r => r.Id).ToArray().Contains(d.RoomId)).ToList();

    //    //Filer de devices waar het account toegang tot heeft
    //    deviceList = deviceList.Where(d => devicesAccessList.Select(a => a.DeviceId).ToArray().Contains(d.Id)).ToList();

    //    return new DeviceListResponse(deviceList);
    //}

    public async Task<DeviceListResponse> GetAllDevices(SmartHomeRequest request)
    {
        await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);
        //Haal de rooms uit de database die in het huis staan
        List<Room> roomsList = new List<Room>();
        roomsList = await _ctx.DbContext.Rooms.Where(r => r.SmartHomeId == request.smartHome).ToListAsync();

        //Haal de apparaten op en filter devices met de roomids die in het huis staan
        List<Device> deviceList = new List<Device>();
        deviceList = await _ctx.DbContext.Devices.ToListAsync();

        //Filter Devices op room die bij een home hoort
        deviceList = deviceList.Where(d => roomsList.Select(r => r.Id).ToArray().Contains(d.RoomId)).ToList();

        return new DeviceListResponse(deviceList);
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        foreach (Device device in request.devices)
        {
            //Update de device list met de propperties naar de DataBase

            await _ctx.DbContext.Devices
                .Where(d => d.Id == device.Id)
                .ExecuteUpdateAsync(u => u
                    .SetProperty(p => p.Name, device.Name)
                    .SetProperty(p => p.JsonObjectConfig, device.JsonObjectConfig)
                    .SetProperty(p => p.RoomId, device.RoomId)
                    .SetProperty(p => p.Type, device.Type)
                );
        }

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        //Controleer of er al een device met dezelfde naam in de database is
        if (await _ctx.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
            return SuccessResponse.Failed("There is already a device with the same name!!");

        await _ctx.DbContext.Devices
            .Where(d => d.Id == request.device.Id)
                .ExecuteUpdateAsync(u => u
                .SetProperty(p => p.Name, request.device.Name)
                .SetProperty(p => p.JsonObjectConfig, request.device.JsonObjectConfig)
                .SetProperty(p => p.RoomId, request.device.RoomId)
                .SetProperty(p => p.Type, request.device.Type)
            );

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        //Verwijder device uit de database met guid
        await _ctx.DbContext.Devices.Where(d => d.Id == request.DeviceGuid).ExecuteDeleteAsync();

        return SuccessResponse.Success();
    }

    public async Task<GuidResponse> CreateDevice(DeviceRequest request)
    {
        //Controleer of er al een device met dezelfde naam in de database is
        if (await _ctx.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
            return GuidResponse.Failed("There is already a device with the same name!!");

        //Maak een nieuwe device in de database
        var result = await _ctx.DbContext.Devices.AddAsync(request.device);
        await _ctx.DbContext.SaveChangesAsync();

        return new GuidResponse(result.Entity.Id);
    }


    public async Task<RoomListResponse> GetRoomsByHouseId(SmartHomeRequest request)
    {
        var result = await _ctx.DbContext.Rooms.ToListAsync();

        if (result == null)
            return RoomListResponse.Failed("Not Devices found in DataBase");
        else
            return new RoomListResponse(result);
    }

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        var result = await _ctx.DbContext.Devices
            .Where(d => d.Id == request.DeviceId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.JsonObjectConfig, request.ConfigJson));

        return SuccessResponse.Success();
    }
}
