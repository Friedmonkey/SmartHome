using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using System.Security.Cryptography.Xml;
using static SmartHome.Common.Api.IDeviceService;
//using static SmartHome.Common.SharedConfig.Urls;

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
        
        //_ctx.CreateLog("User", request, LogType.Action);

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

        Dictionary<Guid, Room> Rooms = new Dictionary<Guid, Room>();
        foreach (var device in deviceList)
        {
            if (!Rooms.TryGetValue(device.RoomId, out Room? room))
                room = await _ctx.DbContext.Rooms.FirstOrDefaultAsync(r => r.Id == device.RoomId && r.SmartHomeId == request.smartHome);
            if (room is null)
                throw new ApiError("Room not found!");

            Rooms[device.RoomId] = room;
            device.Room = room;
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
            await _ctx.Device.UpdateDeviceSafe(request.smartHome, device, smartUser);
        }

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        //var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        SmartUserModel smartUser1 = new SmartUserModel();
        smartUser1.SmartHomeId = Guid.Parse("054aba40-97d2-4b85-8269-35206b8141b7");
        smartUser1.Id = Guid.Parse("08dd5263-9f14-4cfc-805c-1a20fd81fbca");
        smartUser1.AccountId = Guid.Parse("08dd5263-971a-4292-88e4-cbfa8f390874");

        await _ctx.Device.UpdateDeviceSafe(request.smartHome, request.device, smartUser1);

        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        //await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
       // await _ctx.Device.EnforceDeviceInSmartHome(request.smartHome, request.DeviceGuid);

        await _ctx.DbContext.Devices.Where(d => d.Id == request.DeviceGuid).ExecuteDeleteAsync();
        return SuccessResponse.Success();
    }
    public async Task<GuidResponse> CreateDevice(DeviceRequest request)
    {
        //await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        _ctx.Device.EnforceCorrectDeviceType(request.device.Type);

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

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);
        var device = await _ctx.Device.GetDeviceWithAccess(smartUser, request.DeviceId);

        device.JsonObjectConfig = request.ConfigJson;

        await _ctx.DbContext.SaveChangesAsync();

        return SuccessResponse.Success();
    }
}
