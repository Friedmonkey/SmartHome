using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.UI.Api
{
    public class InternalDeviceService : IDeviceService
    {
        private readonly ApiService _api;

        public InternalDeviceService(ApiService api)
        {
            this._api = api;
        }

        public async Task<DiviceListResponse> GetDevicesWithAcces(DeviceListRequest request)
        {
            return await _api.Get<DiviceListResponse>(SharedConfig.Urls.Device.GetDevicesWithAcces, request, false);
        }

        public async Task<DiviceListResponse> GetAllDevices(AllDeviceListRequest request)
        {
            return await _api.Get<DiviceListResponse>(SharedConfig.Urls.Device.GetAllDevices, request, false);
        }

        public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevicesRange, request, false);
        }

        public async Task<SuccessResponse> UpdateDevice(UpdateDeviceRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevice, request, false);
        }

        public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
        {
            return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Device.DeleteDevice, request, false);
        }

        public async Task<SuccessResponse> CreateDevice(CreateDeviceRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.CreaateDevice, request, false);
        }

        public async Task<RoomListResponse> GetRoomsByHouseId(RoomListRequest request)
        {
            return await _api.Get<RoomListResponse>(SharedConfig.Urls.Device.GetAllRooms, request, false);
        }

        public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDeviceConfig, request, false);
        }
    }
}
