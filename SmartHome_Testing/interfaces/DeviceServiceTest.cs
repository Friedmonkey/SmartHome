using Newtonsoft.Json;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Configs;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartHome.Common.Api.IDeviceService;
//using static SmartHome.Common.SharedConfig.Urls;

namespace SmartHome_Testing.interfaces
{
    public class DeviceServiceTest : IDeviceService
    {
        public List<Device> TestDevices = new List<Device>();

        public async Task<DeviceListResponse> GetAllDevices(EmptySmartHomeRequest request)
        {
            
            return new DeviceListResponse(TestDevices);
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
