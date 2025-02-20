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
        object cacheKey = new { id = request.Id };
        TimeSpan cacheTime = TimeSpan.FromMinutes(2);
        return await _api.GetWithCache<SmartHomeResponse>(cacheKey, SharedConfig.Urls.SmartHome.GetByIDUrl, request, cacheTime);
    }

    public async Task<UserListResponse> GetAllUsers(EmptySmartHomeRequest request)
    {
        object cacheKey = new { };
        TimeSpan cacheTime = TimeSpan.FromMinutes(5);
        return await _api.GetWithCache<UserListResponse>(cacheKey, SharedConfig.Urls.SmartHome.GetAllUsers, request, cacheTime);
    }
}
