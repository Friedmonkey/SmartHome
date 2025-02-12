using SmartHome.Common.Models;

namespace SmartHome.Common.Api;


public interface ISmartHomeService
{
    public record SmartHomeResponse() : Response<SmartHomeResponse>;
    public record CreateSmartHomeRequest(string name, string ssId, string ssPassword);
    public record RequestByGuid(Guid Id);
    
    public Task<SuccessResponse> CreateSmartHome(CreateSmartHomeRequest request);
    
    public Task<SuccessResponse> GetSmartHomesOfSmartUser(RequestByGuid request);
    
    public Task<SuccessResponse> DeleteSmartHome(RequestByGuid request);
    
    public Task<SuccessResponse> UpdateSmartHome(CreateSmartHomeRequest request);


}
