using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Api.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Api.Common;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.Api;

public class DeviceService : IDeviceService
{
    private readonly ApiContext _ctx;

    public DeviceService(ApiContext context)
    {
        _ctx = context;
    }

    public async Task<DeviceListResponse> GetAllDevices(EmptySmartHomeRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        List<Device>? deviceList = null;
        if (smartUser.Role == UserRole.Admin)
        {   //get all no checking exept for smarthome
            deviceList = await _ctx.DbContext.Devices
                .Where(d =>
                    _ctx.DbContext.Rooms
                        .Where(r => r.SmartHomeId == request.smartHome)
                        .Select(r => r.Id)
                        .Contains(d.RoomId)
                ).ToListAsync();
        }
        else
        {   // Get all devices with access and stuff
            deviceList = await _ctx.DbContext.Devices
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
        }
        if (deviceList is null)
            return DeviceListResponse.Failed("deviceList was null, unhandled role?");
        return new DeviceListResponse(deviceList);
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);
        foreach (Device device in request.devices)
        {
            await UpdateDeviceSafe(smartUser, device);

            ////Update de device list met de propperties naar de DataBase

            //await _ctx.DbContext.Devices
            //    .Where(d => d.Id == device.Id)
            //    .ExecuteUpdateAsync(u => u
            //        .SetProperty(p => p.Name, device.Name)
            //        .SetProperty(p => p.JsonObjectConfig, device.JsonObjectConfig)
            //        .SetProperty(p => p.RoomId, device.RoomId)
            //        .SetProperty(p => p.Type, device.Type)
            //    );
        }

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        await UpdateDeviceSafe(smartUser, request.device);
        ////Controleer of er al een device met dezelfde naam in de database is
        //if (await _ctx.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
        //    return SuccessResponse.Failed("There is already a device with the same name!!");

        //await _ctx.DbContext.Devices
        //    .Where(d => d.Id == request.device.Id)
        //        .ExecuteUpdateAsync(u => u
        //        .SetProperty(p => p.Name, request.device.Name)
        //        .SetProperty(p => p.JsonObjectConfig, request.device.JsonObjectConfig)
        //        .SetProperty(p => p.RoomId, request.device.RoomId)
        //        .SetProperty(p => p.Type, request.device.Type)
        //    );

        return SuccessResponse.Success();
    }
    private async Task<SuccessResponse> UpdateDeviceSafe(SmartUserModel smartUser, Device updateDevice)
    {



            //get device from database using device.Id because the other data the user has sent cannot be trusted

            //use the new device and check the room id and stuff

            //from device to room to smart home
            return SuccessResponse.Failed("Not implemented yet");
        //
        ////Controleer of er al een device met dezelfde naam in de database is
        //if (await _ctx.DbContext.Devices.AnyAsync(x => x.Name == request.device.Name))
        //    return SuccessResponse.Failed("There is already a device with the same name!!");

        //await _ctx.DbContext.Devices
        //    .Where(d => d.Id == request.device.Id)
        //        .ExecuteUpdateAsync(u => u
        //        .SetProperty(p => p.Name, request.device.Name)
        //        .SetProperty(p => p.JsonObjectConfig, request.device.JsonObjectConfig)
        //        .SetProperty(p => p.RoomId, request.device.RoomId)
        //        .SetProperty(p => p.Type, request.device.Type)
        //    );
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


    public async Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request)
    {
        await _ctx.Auth.EnforceIsPartOfSmartHome(request.smartHome);
        var result = await _ctx.DbContext.Rooms.Where(r => r.SmartHomeId == request.smartHome).ToListAsync();

        if (result == null)
            return RoomListResponse.Failed("No Rooms found in DataBase");
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
