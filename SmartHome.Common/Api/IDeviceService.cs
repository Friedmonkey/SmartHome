using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Api
{
    public record DiviceListResponse(List<Device> Devices) : Response<DiviceListResponse>;

    public interface IDeviceService
    {
        ///Maak een Response aan
        public record DeviceListRequest(string HomeGuid);
        Task<DiviceListResponse> GetDevicesByHouseId(DeviceListRequest request);

        public record UpdateDeviceConfigRequest(string DeviceId, string ConfigJson);
        Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request);
    }
}
