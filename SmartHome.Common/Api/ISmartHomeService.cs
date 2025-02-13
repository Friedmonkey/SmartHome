using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface ISmartHomeService
{
    public record SmartHomeResponse(List<SmartHomeModel> homes) : Response<SmartHomeResponse>;

    public record CreateSmartHomeRequest(string name);
    public Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request);

    public record InviteRequest(Guid smartHome, string email);
    public Task<SuccessResponse> InviteToSmartHome(InviteRequest request);

    public record AcceptInviteRequest(Guid smartHome);
    public Task<SuccessResponse> AcceptSmartHomeInvite(AcceptInviteRequest request);

    public Task<SmartHomeResponse> GetJoinedSmartHomes(EmptyRequest request);
    public Task<SmartHomeResponse> GetSmartHomeInvites(EmptyRequest request);
}
