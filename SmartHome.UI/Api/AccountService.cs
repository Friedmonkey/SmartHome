using System.Net.Http.Json;
using Microsoft.JSInterop;

namespace SmartHome.UI.Api;

public class AccountService
{
    private readonly ApiService _apiService;
    private readonly IJSRuntime _jsRuntime;

    private const string JWT_STORAGE_KEY = "jwt_token";
    private const string REFRESH_STORAGE_KEY = "refresh_token";

    public AccountService(ApiService apiService, IJSRuntime jsRuntime)
    {
        _apiService = apiService;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> Login(string email, string password)
    {
        var response = await _apiService.Post<TokenResponse>("/auth/login", new { email, password });

        if (response != null)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", JWT_STORAGE_KEY, response.Jwt);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REFRESH_STORAGE_KEY, response.RefreshToken);
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", JWT_STORAGE_KEY);
        await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", REFRESH_STORAGE_KEY);
    }

    public async Task<bool> IsLoggedIn()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", JWT_STORAGE_KEY);
        return !string.IsNullOrEmpty(token);
    }
}
