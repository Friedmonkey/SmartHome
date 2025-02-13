using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public interface IDeviceService
{
    public record Response(object Device) : Response<Response>;
    public record DevicesResponse(List<object> Devices) : Response<DevicesResponse>;
    
    public record CreateRequest(string Name, Guid SmartHomeId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<DevicesResponse> GetDevicesOfSmartHome(GuidRequest request); // return list of Devices
    
    public Task<SuccessResponse> Delete(GuidRequest request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
