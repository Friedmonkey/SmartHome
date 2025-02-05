using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;

namespace SmartHome.UI.Api;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly IJSRuntime _jsRuntime;
    private readonly FrontendConfig _config;

    private const string JWT_STORAGE_KEY = "jwt_token";
    private const string REFRESH_STORAGE_KEY = "refresh_token";

    public ApiService(HttpClient httpClient, IJSRuntime jsRuntime, FrontendConfig config)
    {
        _httpClient = httpClient;
        _jsRuntime = jsRuntime;
        _config = config;
    }

    public async Task<T?> Get<T>(string url)
    {
        await EnsureAuthenticated();
        var request = new HttpRequestMessage(HttpMethod.Get, _config.ApiBaseUrl + url);
        await AttachAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);

        return await HandleResponse<T>(response);
    }

    public async Task<T?> Post<T>(string url, object data)
    {
        await EnsureAuthenticated();
        var request = new HttpRequestMessage(HttpMethod.Post, _config.ApiBaseUrl + url)
        {
            Content = JsonContent.Create(data)
        };
        await AttachAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);

        return await HandleResponse<T>(response);
    }

    public async Task<T?> Put<T>(string url, object data)
    {
        await EnsureAuthenticated();
        var request = new HttpRequestMessage(HttpMethod.Put, _config.ApiBaseUrl + url)
        {
            Content = JsonContent.Create(data)
        };
        await AttachAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);

        return await HandleResponse<T>(response);
    }

    public async Task<T?> Delete<T>(string url)
    {
        await EnsureAuthenticated();
        var request = new HttpRequestMessage(HttpMethod.Delete, _config.ApiBaseUrl + url);
        await AttachAuthorizationHeader(request);
        var response = await _httpClient.SendAsync(request);

        return await HandleResponse<T>(response);
    }

    private async Task AttachAuthorizationHeader(HttpRequestMessage request)
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", JWT_STORAGE_KEY);
        if (!string.IsNullOrEmpty(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private async Task EnsureAuthenticated()
    {
        var token = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", JWT_STORAGE_KEY);
        if (string.IsNullOrEmpty(token) || IsTokenExpired(token))
        {
            await RefreshToken();
        }
    }

    private bool IsTokenExpired(string token)
    {
        try
        {
            var payload = token.Split('.')[1]; // JWT payload
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            var json = JsonSerializer.Deserialize<JwtPayload>(jsonBytes);
            return json?.Exp < DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }
        catch
        {
            return true; // Assume expired if decoding fails
        }
    }

    private async Task RefreshToken()
    {
        var refreshToken = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", REFRESH_STORAGE_KEY);
        if (string.IsNullOrEmpty(refreshToken)) return;

        var response = await _httpClient.PostAsJsonAsync(_config.ApiBaseUrl + "/auth/refresh", new { refreshToken });

        if (response.IsSuccessStatusCode)
        {
            var newTokens = await response.Content.ReadFromJsonAsync<TokenResponse>();
            if (newTokens != null)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", JWT_STORAGE_KEY, newTokens.Jwt);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REFRESH_STORAGE_KEY, newTokens.RefreshToken);
            }
        }
    }

    private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Error: {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

    private static string PadBase64(string base64)
    {
        return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
    }
}

public class JwtPayload
{
    public long Exp { get; set; }
}

public class TokenResponse
{
    public string Jwt { get; set; } = "";
    public string RefreshToken { get; set; } = "";
}
