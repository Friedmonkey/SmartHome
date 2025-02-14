using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface ISmartHomeService
{
    public record SmartHomeResponse(List<SmartHomeModel> homes) : Response<SmartHomeResponse>;

    public record CreateSmartHomeRequest(string name, string wifiname, string password);
    public Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request);

    public record InviteRequest(Guid smartHome, string email);
    public Task<SuccessResponse> InviteToSmartHome(InviteRequest request);

    public Task<SuccessResponse> AcceptSmartHomeInvite(SmartHomeRequest request);

    public Task<SmartHomeResponse> GetJoinedSmartHomes(EmptyRequest request);
    public Task<SmartHomeResponse> GetSmartHomeInvites(EmptyRequest request);
}
