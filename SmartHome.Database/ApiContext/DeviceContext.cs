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

    public async Task UpdateDeviceSafe(Device updateDevice, Guid smartUserId)
    {
        throw new NotImplementedException(nameof(UpdateDeviceSafe));
        Device device = await GetDeviceWithAccess(updateDevice.Id, smartUserId);
        //we know this is the correct device and we know we have access to it

        //check if device with name excists
        //check room
        //check smarthome
        //name only needs to be unique between smarthome itself i thnik
        bool nameIsUsed = true;// await _dbContext.Devices.AnyAsync(d => (d.) && d.Name == );
        if (nameIsUsed)
            throw new ApiError($"Device with name {updateDevice.Name} already exists!");
        //DbContext.Devices.ExecuteUpdateAsync(idk => updateDevice);
    }
    public async Task<Device> GetDeviceWithAccess(Guid deviceid, Guid smartUserId)
    {
        if (!(await HasAccessToDevice(smartUserId, deviceid)))
            throw new ApiError("User does not have access to device!");
        return await GetDevice(deviceid);
    }
    public async Task<Device> GetDevice(Guid deviceid)
    {
        Device? device = await _dbContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceid);
        if (device is null)
            throw new ApiError("Device not found");

        return device;
    }
    public async Task<bool> HasAccessToDevice(Guid smartUserId, Guid deviceid)
    {
        var result = await _dbContext.DeviceAccesses.AnyAsync(da => da.SmartUserId == smartUserId && da.DeviceId == deviceid);
        return result;
    }
}
