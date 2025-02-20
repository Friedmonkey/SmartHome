using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using Device = SmartHome.Common.Models.Entities.Device;

namespace SmartHome.Database.ApiContext;

public class DeviceContext
{
    private readonly SmartHomeContext _dbContext;

    public DeviceContext(SmartHomeContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateDeviceSafe(Guid smartHomeId, Device updateDevice, Guid smartUserId)
    {
        Device existingDevice = await GetDeviceWithAccess(updateDevice.Id, smartUserId);

        await EnforceDeviceNameUnique(smartHomeId, updateDevice.Name);
        existingDevice.Name = updateDevice.Name;
        existingDevice.Type = updateDevice.Type;
        existingDevice.JsonObjectConfig = updateDevice.JsonObjectConfig;

        //so if you rename multiple the unique name still works
        await _dbContext.SaveChangesAsync();
    }

    ////we know this is the correct device and we know we have access to it

    ////check if device with name excists
    ////check room
    ////check smarthome
    ////name only needs to be unique between smarthome itself i thnik
    //bool nameIsUsed = true;// await _dbContext.Devices.AnyAsync(d => (d.) && d.Name == );
    //    if (nameIsUsed)
    //        throw new ApiError($"Device with name {updateDevice.Name} already exists!");
    ////DbContext.Devices.ExecuteUpdateAsync(idk => updateDevice);
    //public async Task<Device> GetDevicesFromSmartHome(Guid smartHomeId)
    //{ 
    //    _dbContext.Rooms
    //}
    public async Task EnforceDeviceNameUnique(Guid smartHomeId, string? deviceName)
    {
        if (string.IsNullOrEmpty(deviceName))
            throw new ApiError("Device name cannot be empty!");

        bool alreadyExists = await _dbContext.Devices
            .Where(d => _dbContext.Rooms
                .Where(r => r.SmartHomeId == smartHomeId)
                .Select(r => r.Id)
                .Contains(d.RoomId))
            .AnyAsync(d => d.Name == deviceName);

        if (alreadyExists)
            throw new ApiError("There is already a device with the same name!!");
    }
    public async Task<bool> IsRoomInSmartHome(Guid smartHomeId, Guid roomId)
    {
        return await _dbContext.Rooms
            .AnyAsync(r => r.Id == roomId && r.SmartHomeId == smartHomeId);
    }
    public async Task EnforceDeviceInSmartHome(Guid smartHomeId, Guid deviceId)
    {
        bool isInSmartHome = await _dbContext.Devices
            .AnyAsync(d => d.Id == deviceId && _dbContext.Rooms
                .Where(r => r.SmartHomeId == smartHomeId)
                .Select(r => r.Id)
                .Contains(d.RoomId));

        if (!isInSmartHome)
            throw new ApiError("The device does not exist within the given smarthome!");
    }

    public async Task<Device> GetDeviceWithAccess(Guid deviceid, Guid smartUserId)
    {
        await EnforceHasAccessToDevice(smartUserId, deviceid);
        return await GetDevice(deviceid);
    }
    public async Task<Device> GetDevice(Guid deviceid)
    {
        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceid);
        if (device is null)
            throw new ApiError("Device not found");

        return device;
    }
    public async Task EnforceHasAccessToDevice(Guid smartUserId, Guid deviceid)
    {
        var hasAccess = await _dbContext.DeviceAccesses.AnyAsync(da => da.SmartUserId == smartUserId && da.DeviceId == deviceid);
        if (!hasAccess)
            throw new ApiError("User does not have access to device or it does not exist!");
    }
}
