using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
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
        
        //_ctx.CreateLog("User", request, LogType.Action);

        List<Device> deviceList = await GetUserDevicesWithAccess(request.smartHome, smartUser);

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

        return new DeviceListResponse(deviceList);
    }

    private async Task<List<Device>> GetUserDevicesWithAccess(Guid smartHomeId, SmartUserModel smartUser)
    {
        List<Device>? deviceList = null;
        if (smartUser.Role == UserRole.Admin)
        {   //get all no checking exept for smarthome
            deviceList = await _ctx.DbContext.Devices
                .Where(d =>
                    _ctx.DbContext.Rooms
                        .Where(r => r.SmartHomeId == smartHomeId)
                        .Select(r => r.Id)
                        .Contains(d.RoomId)
                ).ToListAsync();
        }
        else
        {   // Get all devices with access and stuff
            deviceList = await _ctx.DbContext.Devices
            .Where(d =>
                _ctx.DbContext.Rooms
                    .Where(r => r.SmartHomeId == smartHomeId)
                    .Select(r => r.Id)
                    .Contains(d.RoomId) && // Device belongs to the Smart Home
            _ctx.DbContext.DeviceAccesses
                    .Where(a => a.SmartUserId == smartUser.Id)
                    .Select(a => a.DeviceId)
                    .Contains(d.Id) // User has access to the device
            ).ToListAsync();
        }
        if (deviceList is null)
            throw new ApiError("deviceList was null, unhandled role?");
        return deviceList;
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
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        await _ctx.Device.UpdateDeviceSafe(request.smartHome, request.device, smartUser);

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
    public async Task<UserDevicesAccessAdminResponse> GetUserDevicesAccessAdmin(SmartHomeGuidRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);

        //TODO: make authcontext got getting smartuser by smartuser.id instead of this custom stuff
        var user = await _ctx.DbContext.SmartUsers.FirstOrDefaultAsync(sm => sm.Id == request.Id && sm.SmartHomeId == request.smartHome);
        if (user is null)
            return UserDevicesAccessAdminResponse.Failed("User with guid does not exist in the smarthome");

        var acc = await _ctx.UserManager.Users.FirstOrDefaultAsync(a => a.Id == user.AccountId);
        if (acc is null)
            return UserDevicesAccessAdminResponse.Failed("Failed to get account from user!");

        user.Account = acc;

        //get devices for the user we requested, NOT the logged in user
        var devices = await GetUserDevicesWithAccess(request.smartHome, user);

        return new UserDevicesAccessAdminResponse(user, devices);
    }
    public async Task<SuccessResponse> GiveDevicesAccessAdmin(DeviceAccessRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Auth.EnforceUserIsPartOfSmartHome(request.smartHome, request.userId);

        //TODO: check if the logged in smartuser isnt equal to the one we are requesting
        //var user = await _ctx.Auth.GetSmartUser();
        //if (_ctx.Auth.GetLoggedInId)

        foreach (Guid deviceId in request.deviceIds)
        {
            await _ctx.Device.EnforceDeviceInSmartHome(request.smartHome, deviceId);

            var deviceAccess = new DeviceAccess() 
            {
                DeviceId = deviceId,
                SmartUserId = request.userId,
            };

            _ctx.DbContext.DeviceAccesses.Add(deviceAccess);
        }
        await _ctx.DbContext.SaveChangesAsync();

        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> RevokeDevicesAccessAdmin(DeviceAccessRequest request)
    {
        await _ctx.Auth.EnforceIsSmartHomeAdmin(request.smartHome);
        await _ctx.Auth.EnforceUserIsPartOfSmartHome(request.smartHome, request.userId);
        foreach (Guid deviceId in request.deviceIds)
        { 
            await _ctx.Device.EnforceDeviceInSmartHome(request.smartHome, deviceId);

            await _ctx.DbContext.DeviceAccesses
                .Where(da => da.DeviceId == deviceId && da.SmartUserId == request.userId).ExecuteDeleteAsync();
        }

        return SuccessResponse.Success();
    }
}
