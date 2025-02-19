﻿using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;

namespace SmartHome.Backend.FastEndpoints
{
    public class GetDevicesWithAccessEndpoint : BasicEndpointBase<DeviceListRequest, DeviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetDevicesWithAccess);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(DeviceListRequest request, CancellationToken ct)
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

    public class GetAllDevices : BasicEndpointBase<AllDeviceListRequest, DeviceListResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Get(SharedConfig.Urls.Device.GetAllDevices);
            SecureJwtEndpoint();
        }

        public override async Task HandleAsync(AllDeviceListRequest request, CancellationToken ct)
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

    public class UpdateDeviceEndpoint : BasicEndpointBase<UpdateDeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.UpdateDevice);
            SecureJwtEndpoint();
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

    public class CreateDeviceEndpoint : BasicEndpointBase<CreateDeviceRequest, SuccessResponse>
    {
        public required IDeviceService Service { get; set; }
        public override void Configure()
        {
            Post(SharedConfig.Urls.Device.CreaateDevice);
            SecureJwtEndpoint();
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
