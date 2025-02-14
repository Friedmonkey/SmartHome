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
    public async Task<GuidResponse> CreateSmartHome(ISmartHomeService.CreateSmartHomeRequest request)
    {
        return await _api.Post<GuidResponse>(SharedConfig.Urls.SmartHome.CreateSmartHomeUrl, request);
    }

    public async Task<SuccessResponse> InviteToSmartHome(ISmartHomeService.InviteRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.SmartHome.InviteToSmartHomeUrl, request);
    }
    public async Task<SuccessResponse> AcceptSmartHomeInvite(ISmartHomeService.AcceptInviteRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.SmartHome.AcceptInviteToSmartHomeUrl, request);
    }

    public async Task<SmartHomeResponse> GetJoinedSmartHomes(EmptyRequest request)
    {
        return await _api.Get<SmartHomeResponse>(SharedConfig.Urls.SmartHome.getJoinedUrl, request);
    }
    public async Task<SmartHomeResponse> GetSmartHomeInvites(EmptyRequest request)
    {
        return await _api.Get<SmartHomeResponse>(SharedConfig.Urls.SmartHome.getInvitesUrl, request);
    }
}
