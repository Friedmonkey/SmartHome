using MudBlazor;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using static SmartHome.Common.Api.IAccountService;
namespace SmartHome.UI.Api;

public class AccountService : IAccountService
{
    private readonly ApiService _api;

    public AccountService(ApiService api)
    {
        this._api = api;
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var response = await _api.Post<TokenResponse>(SharedConfig.AuthUrls.LoginUrl, request);
        return response;
    }
    public async Task<EmptyResponse> Register(RegisterRequest request)
    {
        return await _api.Post<EmptyResponse>(SharedConfig.AuthUrls.RegisterUrl, request);
    }
    public async Task<EmptyResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        return await _api.Post<EmptyResponse>(SharedConfig.Urls.Account.ForgotPasswordUrl, request);
    }

    public async Task<TokenResponse> Refresh(EmptyRequest request)
    {
        return await _api.Post<TokenResponse>(SharedConfig.AuthUrls.RefreshUrl, request);
    }
}
