using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record DeviceListResponse(List<Device> Devices) : Response<DeviceListResponse>;
public record DeviceRequest(Device device) : SmartHomeRequest;

public interface IDeviceService
{
    Task<DeviceListResponse> GetAllDevices(EmptySmartHomeRequest request);

    public record UpdateDevicesRangeRequest(List<Device> devices) : SmartHomeRequest;
    Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request);

    Task<SuccessResponse> UpdateDevice(DeviceRequest request);

    public record DeleteDeviceRequest(Guid DeviceGuid) : SmartHomeRequest;
    Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request);

    Task<GuidResponse> CreateDevice(DeviceRequest request);

    public record UpdateDeviceConfigRequest(Guid DeviceId, string ConfigJson) : SmartHomeRequest;
    Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request);
}
