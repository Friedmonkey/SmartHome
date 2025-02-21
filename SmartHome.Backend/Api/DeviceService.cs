using Microsoft.EntityFrameworkCore;
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

        List<Room> rooms = new List<Room>();
        rooms = await _ctx.DbContext.Rooms.ToListAsync();

        //Zet de room obejct in de devices
        deviceList = deviceList.Select(d => {
            {
                d.Room =
                new Room
                {
                    Id = rooms.Where(x => x.Id == d.RoomId).ToList().First().Id,
                    Name = rooms.Where(x => x.Id == d.RoomId).ToList().First().Name,
                    
                };
            }
            return d;
        }).ToList();

        if (deviceList is null)
            return DeviceListResponse.Failed("deviceList was null, unhandled role?");
        return new DeviceListResponse(deviceList);
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);
        foreach (Device device in request.devices)
        {
            await _ctx.DbContext.Devices.Where(d => d.Id == device.Id).ExecuteUpdateAsync(setters => setters.SetProperty(d => d.RoomId, device.Room.Id));
        }

        return SuccessResponse.Success();
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        var smartUser = await _ctx.Auth.GetLoggedInSmartUser(request.smartHome);

        if (!_ctx.DbContext.Devices.Where(d => d.Id != request.device.Id).Any(d => d.Name == request.device.Name))
        {
            //Als de device naam niet al bestaaat

            _ctx.DbContext.Devices.Where(d => d.Id == request.device.Id).ExecuteUpdateAsync(setters => setters
            .SetProperty(d => d.Name, request.device.Name)
            .SetProperty(d => d.JsonObjectConfig, request.device.JsonObjectConfig)
            .SetProperty(d => d.RoomId, request.device.RoomId)
            .SetProperty(d => d.Type, request.device.Type)
            );

            return SuccessResponse.Success();
        } else
        {
            return SuccessResponse.Failed("Device naam bestaat al");
        }        
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


        var SmartUserIdList = await _ctx.DbContext.SmartUsers.Where(d => d.SmartHomeId == request.smartHome).Select(d => d.Id).ToListAsync();

        foreach(Guid SmartUserId in SmartUserIdList)
        {
            DeviceAccess deviceAccess = new DeviceAccess();
            deviceAccess.DeviceId = newDevice.Id;
            deviceAccess.SmartUserId = SmartUserId;

            await _ctx.DbContext.DeviceAccesses.AddAsync(deviceAccess);
        }

        await _ctx.DbContext.SaveChangesAsync();

        return new GuidResponse(result.Entity.Id);
    }

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        await _ctx.Device.EnforceHasAccessToDevice(request.smartHome, request.DeviceId);

        var result = await _ctx.DbContext.Devices
            .Where(d => d.Id == request.DeviceId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.JsonObjectConfig, request.ConfigJson));

        return SuccessResponse.Success();
    }
}
