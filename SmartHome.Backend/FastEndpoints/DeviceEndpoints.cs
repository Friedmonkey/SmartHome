using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.FastEndpoints
{
    public class GetDevicesWithAccessEndpoint : BasicEndpointBase<EmptySmartHomeRequest, DeviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetDevicesWithAccess);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetDevicesWithAccess(request));
            }
            catch (Exception ex)
            {
                await SendAsync(DeviceListResponse.Error(ex));
            }
        }
    }

    public class GetAllDevices : BasicEndpointBase<EmptySmartHomeRequest, DeviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetAllDevices);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetAllDevices(request));
            }
            catch (Exception ex)
            {
                await SendAsync(DeviceListResponse.Error(ex));
            }
        }
    }

    public class UpdateDevicesRangeEndpoint : BasicEndpointBase<UpdateDevicesRangeRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDevicesRange);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(UpdateDevicesRangeRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.UpdateDevicesRange(request));
            }
            catch (Exception ex)
            {
                await SendAsync(SuccessResponse.Error(ex));
            }
        }
    }

    public class UpdateDeviceEndpoint : BasicEndpointBase<DeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDevice);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(DeviceRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.UpdateDevice(request));
            }
            catch (Exception ex)
            {
                await SendAsync(SuccessResponse.Error(ex));
            }
        }
    }

    public class DeleteDeviceEndpoint : BasicEndpointBase<DeleteDeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Delete(SharedConfig.Urls.Device.DeleteDevice);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(DeleteDeviceRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.DeleteDevice(request));
            }
            catch (Exception ex)
            {
                await SendAsync(SuccessResponse.Error(ex));
            }
        }
    }

    public class CreateDeviceEndpoint : BasicEndpointBase<DeviceRequest, GuidResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.CreaateDevice);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(DeviceRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.CreateDevice(request));
            }
            catch (Exception ex)
            {
                await SendAsync(GuidResponse.Error(ex));
            }
        }
    }

    public class GetRoomEndpoint : BasicEndpointBase<EmptySmartHomeRequest, RoomListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.GetAllRooms);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(EmptySmartHomeRequest request, CancellationToken ct)
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
            SecureJwtEndpoint();
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
