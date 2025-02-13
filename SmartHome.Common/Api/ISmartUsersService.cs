using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;

namespace SmartHome.Common.Api;

public interface ISmartUserService
{
    public record SmartUserResponse(List<SmartUser> SmartUsers) : Response<SmartUserResponse>;
    
    public record CreateRequest(Guid SmartHomeId, Guid AccountId, UserRole RoleId);
    public record UpdateRequest(Guid id, Guid SmartHomeId, Guid AccountId, UserRole RoleId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<SmartUserResponse> GetSmartUsersOfAccount(RequestByGuid request); // return list of SmartUser
    
    public Task<SuccessResponse> Delete(RequestByGuid request);
    
    public Task<SuccessResponse> Update(UpdateRequest request);


}
