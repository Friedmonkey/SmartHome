using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public interface ISmartHomeService
{
    public record SmartHomeListResponse(List<SmartHomeModel> homes) : Response<SmartHomeListResponse>;

    public record CreateSmartHomeRequest(string name, string wifiname, string password);
    public Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request);

    public record InviteRequest(string email) : SmartHomeRequest;
    public Task<SuccessResponse> InviteToSmartHome(InviteRequest request);

    public Task<SuccessResponse> AcceptSmartHomeInvite(GuidRequest request);

    public Task<SmartHomeListResponse> GetJoinedSmartHomes(EmptyRequest request);
    public Task<SmartHomeListResponse> GetSmartHomeInvites(EmptyRequest request);


    public record SmartHomeResponse(SmartHomeModel smartHome) : Response<SmartHomeResponse>;
    public Task<SmartHomeResponse> GetSmartHomeById(GuidRequest request);
}
