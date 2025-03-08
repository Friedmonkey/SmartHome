using SmartHome.Common.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome_Testing.interfaces
{
    public class DeviceServiceTest : IDeviceService
    {
        public Task<DeviceListResponse> GetAllDevices(EmptySmartHomeRequest request)
        {
            return null;
        }

        public Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
        {
            return null;
        }

        public Task<SuccessResponse> UpdateDevice(DeviceRequest request)
        {
            return null;
        }

        public Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
        {
            return null;
        }

        public Task<GuidResponse> CreateDevice(DeviceRequest request)
        {
            return null;
        }

        public Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
        {
            return null;
        }
    }
}
