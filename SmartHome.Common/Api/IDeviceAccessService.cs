using SmartHome.Common.Models;

namespace SmartHome.Common.Api;


public interface IDeviceAccessService
{
    public record DeviceAccesResponse(List<object> Access) : Response<DeviceAccesResponse>;
    public record CreateDeviceAccesRequest(Guid DeviceId, Guid UserId);
    public record RequestByGuid(Guid Id);
    
    public Task<SuccessResponse> CreateDeviceAcces(CreateDeviceAccesRequest request);
    
    public Task<SuccessResponse> GetDeviceActionOfSmartUser(RequestByGuid request);
    
    public Task<SuccessResponse> DeleteDeviceAcces(RequestByGuid request);
    
    public Task<SuccessResponse> UpdateDeviceAcces(CreateDeviceAccesRequest request);


}
