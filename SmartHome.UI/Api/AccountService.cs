using MudBlazor;
using SmartHome.Common;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Auth;

namespace SmartHome.UI.Api;

public class AccountService
{
    private readonly ISnackbar _snackBar;
    private readonly FrontendConfig _config;
    private readonly ApiService _api;

    public AccountService(ApiService api, ISnackbar snackbar, FrontendConfig config)
    {
        this._api = api;
        this._snackBar = snackbar;
        this._config = config;
    }

    public async Task Login(LoginRequest request)
    {
        var response = await _api.Post<TokenResponse>(SharedConfig.AuthUrls.LoginUrl, request);
        if (response.EnsureSuccess(_snackBar))
        {
            _snackBar.Add(response.JWT);
        }
    }
    public async Task Register(RegisterRequest request)
    {
        var response = await _api.Post<RegisterResponse>(SharedConfig.AuthUrls.RegisterUrl, request);
        response.Show(_snackBar, "Account created!");
    }
    public async Task ForgotPassword(ForgotPasswordRequest request)
    {
        var response = await _api.Post<GenericSuccessResponse>(SharedConfig.Urls.Account.ForgotPasswordUrl, request);
        response.Show(_snackBar, "An email has been sent to you with instructions to reset your password");
    }
}
