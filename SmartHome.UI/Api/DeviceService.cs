using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.UI.Api;

public class DeviceService : IDeviceService
{
    private readonly ApiService _api;

    public DeviceService(ApiService api)
    {
        this._api = api;
    }

    public async Task<DeviceListResponse> GetDevicesWithAccess(DeviceListRequest request)
    {
        return await _api.Get<DeviceListResponse>(SharedConfig.Urls.Device.GetDevicesWithAccess, request);
    }

    public async Task<DeviceListResponse> GetAllDevices(AllDeviceListRequest request)
    {
        return await _api.Get<DeviceListResponse>(SharedConfig.Urls.Device.GetAllDevices, request);
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevicesRange, request);
    }

    public async Task<SuccessResponse> UpdateDevice(UpdateDeviceRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevice, request);
    }

    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Device.DeleteDevice, request);
    }

    public async Task<SuccessResponse> CreateDevice(CreateDeviceRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.CreaateDevice, request);
    }

    public async Task<RoomListResponse> GetRoomsByHouseId(RoomListRequest request)
    {
        return await _api.Get<RoomListResponse>(SharedConfig.Urls.Device.GetAllRooms, request);
    }

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDeviceConfig, request);
    }
}
