﻿using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Api
{
    public record DeviceListResponse(List<Device> Devices) : Response<DeviceListResponse>;

    public interface IDeviceService
    {
        ///Maak een Response aan
        public record DeviceListRequest(Guid HomeGuid);
        Task<DeviceListResponse> GetDevicesWithAccess(DeviceListRequest request);

        public record AllDeviceListRequest(Guid HomeGuid);
        Task<DeviceListResponse> GetAllDevices(AllDeviceListRequest request);

        public record UpdateDevicesRangeRequest(List<Device> devices);
        Task<SuccessResponse> UpdateDevicesRange(UpdateDevicesRangeRequest request);

        public record UpdateDeviceRequest(Device device);
        Task<SuccessResponse> UpdateDevice(UpdateDeviceRequest request);

        public record DeleteDeviceRequest(Guid DeviceGuid);
        Task<SuccessResponse> DeleteDevice(DeleteDeviceRequest request);

        public record CreateDeviceRequest(Device device);
        Task<SuccessResponse> CreateDevice(CreateDeviceRequest request);

        public record UpdateDeviceConfigRequest(Guid DeviceId, string ConfigJson);
        Task<SuccessResponse> UpdateDeviceConfig(UpdateDeviceConfigRequest request);
    }
}
