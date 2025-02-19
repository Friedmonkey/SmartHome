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
    public async Task<SuccessResponse> AcceptSmartHomeInvite(GuidRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.SmartHome.AcceptInviteToSmartHomeUrl, request);
    }

    public async Task<SmartHomeListResponse> GetJoinedSmartHomes(EmptyRequest request)
    {
        TimeSpan cacheTime = TimeSpan.FromMinutes(2);
        object cacheKey = new { };
        return await _api.GetWithCache<SmartHomeListResponse>(cacheKey, SharedConfig.Urls.SmartHome.GetJoinedUrl, request, cacheTime);
    }
    public async Task<SmartHomeListResponse> GetSmartHomeInvites(EmptyRequest request)
    {
        TimeSpan cacheTime = TimeSpan.FromMinutes(5);
        object cacheKey = new { };
        return await _api.GetWithCache<SmartHomeListResponse>(cacheKey, SharedConfig.Urls.SmartHome.GetInvitesUrl, request, cacheTime);
    }

    public async Task<SmartHomeResponse> GetSmartHomeById(GuidRequest request)
    {
        return await _api.Get<SmartHomeResponse>(SharedConfig.Urls.SmartHome.GetByIDUrl, request);
    }
}
