using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface ISmartHomeService
{
    public record SmartHomeResponse(List<Models.Entities.SmartHome> homes) : Response<SmartHomeResponse>;


    public record CreateSmartHomeRequest(string name);
    public Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request);

    public record InviteToSmartHomeRequest(string email);
    public Task<SuccessResponse> InviteToSmartHome(InviteToSmartHomeRequest request);

    public Task<SmartHomeResponse> GetSmartHomesOfSmartUser(GuidRequest request);
    
    public Task<SuccessResponse> DeleteSmartHome(GuidRequest request);
    

    public record UpdateSmartHomeRequest(Guid Id, string name, string ssId, string ssPassword);    
    public Task<SuccessResponse> UpdateSmartHome(UpdateSmartHomeRequest request);
}
