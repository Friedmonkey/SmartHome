﻿using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IDeviceService;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.UI.Api;

public class DeviceService : IDeviceService
{
    private readonly ApiService _api;

    public DeviceService(ApiService api)
    {
        this._api = api;
    }

    public async Task<DeviceListResponse> GetAllDevices(EmptySmartHomeRequest request)
    {
        object cacheKey = new { };
        TimeSpan cacheTime = TimeSpan.FromMinutes(5);
        return await _api.GetWithCache<DeviceListResponse>(cacheKey, SharedConfig.Urls.Device.GetAllDevices, request, cacheTime);
    }

    public async Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevicesRange, request);
    }

    public async Task<SuccessResponse> UpdateDevice(DeviceRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDevice, request);
    }

    public async Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request)
    {
        return await _api.Delete<SuccessResponse>(SharedConfig.Urls.Device.DeleteDevice, request);
    }

    public async Task<GuidResponse> CreateDevice(DeviceRequest request)
    {
        return await _api.Post<GuidResponse>(SharedConfig.Urls.Device.CreaateDevice, request);
    }

    public async Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Device.UpdateDeviceConfig, request);
    }
}
