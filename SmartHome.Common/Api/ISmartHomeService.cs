using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface ISmartHomeService
{
    public record SmartHomeResponse(List<Home> homes) : Response<SmartHomeResponse>;
    public record CreateSmartHomeRequest(string name, string ssId, string ssPassword);
    public record UpdateSmartHomeRequest(Guid Id, string name, string ssId, string ssPassword);    
    public Task<SuccessResponse> CreateSmartHome(CreateSmartHomeRequest request);
    
    public Task<SmartHomeResponse> GetSmartHomesOfSmartUser(SmartHome.Common.Api.RequestByGuid request);
    
    public Task<SuccessResponse> DeleteSmartHome(RequestByGuid request);
    
    public Task<SuccessResponse> UpdateSmartHome(UpdateSmartHomeRequest request);


}
