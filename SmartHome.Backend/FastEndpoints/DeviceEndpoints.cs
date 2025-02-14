using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;
using static SmartHome.Common.Api.IPersonTestingService;

namespace SmartHome.Backend.FastEndpoints
{
    public class GetDevicesEndpoint : BasicEndpointBase<DeviceListRequest, DiviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetAllDevices);
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeviceListRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetDevicesByHouseId(request));
            }
            catch (Exception ex)
            {
                await SendAsync(DiviceListResponse.Error(ex));
            }
        }
    }

    public class GetRoomEndpoint : BasicEndpointBase<RoomListRequest, RoomListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetAllRooms);
            AllowAnonymous();
        }

        public override async Task HandleAsync(RoomListRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetRoomsByHouseId(request));
            }
            catch (Exception ex)
            {
                await SendAsync(RoomListResponse.Error(ex));
            }
        }
    }

    public class UpdateDivicesConfigEndpoint : BasicEndpointBase<UpdateDeviceConfigRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDeviceConfig);
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateDeviceConfigRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.UpdateDeviceConfig(request));
            }
            catch (Exception ex)
            {
                await SendAsync(SuccessResponse.Error(ex));
            }
        }
    }
}
