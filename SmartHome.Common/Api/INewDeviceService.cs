using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record DeviceListResponse(Dictionary<Room, Device> roomAndDeviceDict) : Response<DeviceListResponse>;
public interface INewDeviceService
{
    public Task<DeviceListResponse> GetRoomsAndDevicesForUser(SmartHomeRequest request);


    public record CreateDeviceRequest(Guid smartHome, string deviceName, string deviceType); //use enum for type
    public Task<GuidResponse> CreateDevice(CreateDeviceRequest request);
}
