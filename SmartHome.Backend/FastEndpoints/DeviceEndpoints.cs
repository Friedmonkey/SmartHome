using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;
using static SmartHome.Common.Api.IPersonTestingService;

namespace SmartHome.Backend.FastEndpoints
{
    public class GetDevicesWidtAccesEndpoint : BasicEndpointBase<DeviceListRequest, DiviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetDevicesWithAcces);
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeviceListRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetDevicesWithAcces(request));
            }
            catch (Exception ex)
            {
                await SendAsync(DiviceListResponse.Error(ex));
            }
        }
    }

    public class GetAllDevices : BasicEndpointBase<AllDeviceListRequest, DiviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetAllDevices);
            AllowAnonymous();
        }

        public override async Task HandleAsync(AllDeviceListRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.GetAllDevices(request));
            }
            catch (Exception ex)
            {
                await SendAsync(DiviceListResponse.Error(ex));
            }
        }
    }

    public class UpdateDevicesRangeEndpoint : BasicEndpointBase<UpdateDevicesRangeRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDevicesRange);
            AllowAnonymous();
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

    public class UpdateDeviceEndpoint : BasicEndpointBase<UpdateDeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDevice);
            AllowAnonymous();
        }

        public override async Task HandleAsync(UpdateDeviceRequest request, CancellationToken ct)
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
            AllowAnonymous();
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

    public class CreateDeviceEndpoint : BasicEndpointBase<CreateDeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.CreaateDevice);
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateDeviceRequest request, CancellationToken ct)
        {
            try
            {
                await SendAsync(await Service.CreateDevice(request));
            }
            catch (Exception ex)
            {
                await SendAsync(SuccessResponse.Error(ex));
            }
        }
    }

    public class GetRoomEndpoint : BasicEndpointBase<RoomListRequest, RoomListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.GetAllRooms);
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
