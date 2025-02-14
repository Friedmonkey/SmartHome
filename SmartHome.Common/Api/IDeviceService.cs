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

    public record RoomListResponse(List<Room> Rooms) : Response<RoomListResponse>;

    public interface IDeviceService
    {
        ///Maak een Response aan
        public record DeviceListRequest(Guid HomeGuid);
        Task<DiviceListResponse> GetDevicesByHouseId(DeviceListRequest request);

        public record RoomListRequest(Guid HomeGuid);
        Task<RoomListResponse> GetRoomsByHouseId(RoomListRequest request);

        public record UpdateDeviceConfigRequest(Guid DeviceId, string ConfigJson);
        Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request);
    }
}
