using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.JSInterop;
using MudBlazor;
using SmartHome.Common;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Auth;

namespace SmartHome.UI.Api;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJSRuntime _jsRuntime;
    private readonly ISnackbar _snackbarService;
    private readonly FrontendConfig _config;

    private const string JWT_STORAGE_KEY = "jwt_token";
    private const string REFRESH_STORAGE_KEY = "refresh_token";

    public ApiService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, ISnackbar snackbarService, FrontendConfig config)
    {
        _httpClientFactory = httpClientFactory;
        _jsRuntime = jsRuntime;
        _snackbarService = snackbarService;
        _config = config;
    }
    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient(_config.HttpClientName);
    }
    private string GetUrl(string url)
    {
        return $"{_config.ApiBaseUrl}/{url}";
    }
    public async Task<T?> Get<T>(string url) where T : Response<T> => await Send<T>(HttpMethod.Get, url);
    public async Task<T?> Post<T>(string url, object data) where T : Response<T> => await Send<T>(HttpMethod.Post, url, data);
    public async Task<T?> Put<T>(string url, object data) where T : Response<T> => await Send<T>(HttpMethod.Put, url, data);
    public async Task<T?> Delete<T>(string url) where T : Response<T> => await Send<T>(HttpMethod.Delete, url);
    private async Task<T?> Send<T>(HttpMethod method, string url, object data = null)
    { 
        await EnsureAuthenticated();
        return await SendInternal<T>(true, method, url, data);
    }
    private async Task<T?> SendInternal<T>(bool authenticate, HttpMethod method, string url, object data = null)
    {
        try
        {
            var newUrl = GetUrl(url);
            var request = new HttpRequestMessage(method, newUrl);
            if (authenticate)
            {
                await AttachAuthorizationHeader(request);
            }
            if (data is not null)
                request.Content = JsonContent.Create(data);

            var response = await HandleResponse<T>(await GetHttpClient().SendAsync(request));
            return response;
        }
        catch (Exception ex)
        {
#if DEBUG
            if (ex.Message.StartsWith("TypeError: Failed to fetch"))
            { 
                _snackbarService.Add("BACKEND not enabled", Severity.Error);
                _snackbarService.Add("TURN ON THE BACK-END, \n Solution Explorer -> SmartHome.Backend -> r-click -> debug -> start new instance", Severity.Error);
                return default(T);
            }
#endif
            throw new Exception("Api error: " + ex.Message);
        }
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

        var refreshRequest = new RefreshRequest(refreshToken);
        var response = await SendInternal<RefreshResponse>(false, HttpMethod.Post, SharedConfig.RefreshUrl, refreshRequest);

        if (response?.Success == true)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", JWT_STORAGE_KEY, response.JWT);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", REFRESH_STORAGE_KEY, response.RefreshToken);
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
