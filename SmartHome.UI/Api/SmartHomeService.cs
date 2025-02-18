using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.UI.Api;

public class SmartHomeService : ISmartHomeService
{
    private readonly ApiService _api;

    public SmartHomeService(ApiService api)
    {
        this._api = api;
    }
    public async Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request)
    {
        return await _api.Post<GuidResponse>(SharedConfig.Urls.SmartHome.CreateSmartHomeUrl, request);
    }

    public async Task<SuccessResponse> InviteToSmartHome(InviteRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.SmartHome.InviteToSmartHomeUrl, request);
    }
    public async Task<SuccessResponse> AcceptSmartHomeInvite(SmartHomeRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.SmartHome.AcceptInviteToSmartHomeUrl, request);
    }

    public async Task<SmartHomeListResponse> GetJoinedSmartHomes(EmptyRequest request)
    {
        return await _api.Get<SmartHomeListResponse>(SharedConfig.Urls.SmartHome.GetJoinedUrl, request);
    }
    public async Task<SmartHomeListResponse> GetSmartHomeInvites(EmptyRequest request)
    {
        return await _api.Get<SmartHomeListResponse>(SharedConfig.Urls.SmartHome.GetInvitesUrl, request);
    }

    public async Task<SmartHomeResponse> GetSmartHomeById(GuidRequest request)
    {
        return await _api.GetSmartHomeById(request);
    }
}
