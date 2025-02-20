using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
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
            await _ctx.Device.UpdateDeviceSafe(request.smartHome, device, smartUser.Id);
        }

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        await _ctx.Device.UpdateDeviceSafe(request.smartHome, request.device, smartUser.Id);

        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Device.EnforceDeviceInSmartHome(request.smartHome, request.DeviceGuid);

        await _ctx.DbContext.Devices.Where(d => d.Id == request.DeviceGuid).ExecuteDeleteAsync();
        return SuccessResponse.Success();
    }
    public async Task<GuidResponse> CreateDevice(DeviceRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);

        if (request.device.RoomId == Guid.Empty)
            return GuidResponse.Failed("Invalid room ID!");

        if (!await _ctx.Device.IsRoomInSmartHome(request.smartHome, request.device.RoomId))
            return GuidResponse.Failed("The room is not part of smart home!");

        await _ctx.Device.EnforceDeviceNameUnique(request.smartHome, request.device.Name);


        Device newDevice = new Device() 
        {
            Name = request.device.Name,
            RoomId = request.device.RoomId,
            Type = request.device.Type,
            JsonObjectConfig = request.device.JsonObjectConfig,
        };
        var result = await _ctx.DbContext.Devices.AddAsync(newDevice);
        await _ctx.DbContext.SaveChangesAsync();

        return new GuidResponse(result.Entity.Id);
    }
    //public async Task<RoomListResponse> GetAllRooms(EmptySmartHomeRequest request)
    //{
    //    await _ctx.Auth.EnforceIsPartOfSmartHome(request.smartHome);
    //    var result = await _ctx.DbContext.Rooms.Where(r => r.SmartHomeId == request.smartHome).ToListAsync();

    //    return new RoomListResponse(result);
    //}
    //        return RoomListResponse.Failed("Not Devices found in DataBase");
    //    else
    //        return new RoomListResponse(result);
    //}

    //        return RoomListResponse.Failed("Not Devices found in DataBase");
    //    else
    //        return new RoomListResponse(result);
    //}

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        await _ctx.Device.EnforceHasAccessToDevice(request.smartHome, request.DeviceId);

        var result = await _ctx.DbContext.Devices
            .Where(d => d.Id == request.DeviceId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.JsonObjectConfig, request.ConfigJson));

        return SuccessResponse.Success();
    }
}
