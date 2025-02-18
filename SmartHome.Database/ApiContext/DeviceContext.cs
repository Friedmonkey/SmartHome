using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using Device = SmartHome.Common.Models.Entities.Device;
using SmartHome.Common.Models.Entities;
using SmartHome.Database.Auth;
using static SmartHome.Common.SharedConfig.Urls;
using SmartHome.Common.Api.Common;

namespace SmartHome.Database.ApiContext;

public class DeviceContext
{
    private readonly SmartHomeContext DbContext;

    public DeviceContext(SmartHomeContext dbContext)
    {
        DbContext = dbContext;
    }

    public async Task UpdateDeviceSafe(Device updateDevice, Guid smartUserId)
    { 
        Device device = await GetDeviceWithAccess(updateDevice.Id, smartUserId);
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
        Device? device = await DbContext.Devices.FirstOrDefaultAsync(d => d.Id == deviceid);
        if (device is null)
            throw new ApiError("Device not found");

        return device;
    }
    public async Task<bool> HasAccessToDevice(Guid smartUserId, Guid deviceid)
    {
        var result = await DbContext.DeviceAccesses.AnyAsync(da => da.SmartUserId == smartUserId && da.DeviceId == deviceid);
        return result;
    }
}
