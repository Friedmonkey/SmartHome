using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IAccountService;
namespace SmartHome.UI.Api;

public class InternalAccountService : IAccountService
{
    private readonly ApiService _api;

    public InternalAccountService(ApiService api)
    {
        this._api = api;
    }

    public async Task<EmptyResponse> Register(RegisterRequest request)
    {
        return await _api.Post<EmptyResponse>(SharedConfig.Urls.Account.RegisterUrl, request, authenticated:false);
    }
    public async Task<TokenResponse> Login(LoginRequest request)
    {
        return await _api.Login(request);
    }
    public async Task<TokenResponse> Refresh(TokenRequest request)
    {
        throw new Exception("dont manually call this!");
        return await _api.Refresh();
    }
    public async Task<EmptyResponse> Logout(EmptyRequest request)
    {
        await _api.Logout();
        return EmptyResponse.Success();
    }
    public async Task<EmptyResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        return await _api.Post<EmptyResponse>(SharedConfig.Urls.Account.ForgotPasswordUrl, request, authenticated:false);
    }
}
