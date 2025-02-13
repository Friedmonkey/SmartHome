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

        public async Task<DiviceListResponse> GetDevicesByHouseId(DeviceListRequest request)
        {
            return await _api.Get<DiviceListResponse>(SharedConfig.Urls.Device.GetAllDevices, request, false);
        }

        public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
        {
            return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDeviceConfig, request, false);
        }

        
    }
}
