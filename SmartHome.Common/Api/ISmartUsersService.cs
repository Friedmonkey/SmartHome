using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public interface ISmartUserService
{
    public record Response(object SmartUser) : Response<Response>;
    public record SmartUserResponse(List<object> SmartUsers) : Response<SmartUserResponse>;
    
    public record CreateRequest(Guid SmartHomeId, Guid AccountId, Guid RoleId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<SuccessResponse> GetSmartUsersOfAccount(RequestByGuid request); // return list of SmartUser
    
    public Task<SuccessResponse> Delete(RequestByGuid request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
