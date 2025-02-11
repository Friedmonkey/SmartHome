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

    public async Task<SuccessResponse> Register(RegisterRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Account.RegisterUrl, request, authenticated:false);
    }
    public async Task<TokenResponse> Login(LoginRequest request)
    {
        return await _api.Login(request);
    }
    public Task<TokenResponse> Refresh(RefreshRequest request)
    {
        throw new Exception("dont manually call this!");
        //return await _api.Refresh();
    }
    public async Task<SuccessResponse> Logout(EmptyRequest request)
    {
        await _api.Logout();
        return SuccessResponse.Success();
    }
    public async Task<SuccessResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Account.ForgotPasswordUrl, request, authenticated:false);
    }
}
