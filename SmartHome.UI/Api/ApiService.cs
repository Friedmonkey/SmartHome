using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.SessionStorage;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.UI.Api;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IJSRuntime _jsRuntime;
    private readonly ISnackbar _snackbarService;
    private readonly FrontendConfig _config;
    private readonly ISessionStorageService _sessionStorageService;

    private const string JWT_KEY = "jwt_token";
    private const string REFRESH_KEY = "refresh_token";
    private JwtSecurityToken? cachedJwt = null;

    public ApiService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime, ISnackbar snackbarService, FrontendConfig config, ISessionStorageService sessionStorageService)
    {
        _httpClientFactory = httpClientFactory;
        _jsRuntime = jsRuntime;
        _snackbarService = snackbarService;
        _config = config;
        _sessionStorageService = sessionStorageService;
    }

    public async Task Logout()
    {
        var response = await Delete<SuccessResponse>(SharedConfig.Urls.Account.LogoutUrl);

        await _sessionStorageService.RemoveItemAsync(JWT_KEY);
        await _sessionStorageService.RemoveItemAsync(REFRESH_KEY);

        cachedJwt = null;
    }
    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var response = await Post<TokenResponse>(SharedConfig.Urls.Account.LoginUrl, request, authenticated: false);

        if (response.EnforceSuccess())
        {
            await _sessionStorageService.SetItemAsync(JWT_KEY, response.JWT);
            await _sessionStorageService.SetItemAsync(REFRESH_KEY, response.Refresh);
            cachedJwt = new JwtSecurityToken(response.JWT);
        }
        
        return response;
    }
    public async Task<TokenResponse> Refresh()
    {
        var jwt = await _sessionStorageService.GetItemAsync<string>(JWT_KEY);
        var refresh = await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY);

        var request = new TokenRequest(JWT: jwt, Refresh: refresh);
        var response = await Post<TokenResponse>(SharedConfig.Urls.Account.RefreshUrl, request, authenticated:false);

        if (response.EnforceSuccess())
        {
            await _sessionStorageService.SetItemAsync(JWT_KEY, response.JWT);
            await _sessionStorageService.SetItemAsync(REFRESH_KEY, response.Refresh);

            cachedJwt = new JwtSecurityToken(response.JWT);
        }

        return response;
    }
    public async Task<T> Get<T>(string url, object data = null, bool authenticated = true) where T : Response<T>
    {
        if (data is not null)
        {
            // Convert the data object to a query string
            var queryString = ConvertToQueryString(data);
            url = $"{url}?{queryString}";
        }
        return await Send<T>(authenticated, HttpMethod.Get, url, null);
    }
    public async Task<T> Post<T>(string url, object data, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Post, url, data);
    }
    public async Task<T> Put<T>(string url, object data, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Put, url, data);
    }
    public async Task<T> Delete<T>(string url, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Delete, url);
    }
    private string ConvertToQueryString(object data)
    {
        var queryString = new StringBuilder();

        // Serialize the object into key-value pairs for query params
        foreach (var property in data.GetType().GetProperties())
        {
            var value = property.GetValue(data);
            if (value != null)
            {
                queryString.Append($"{Uri.EscapeDataString(property.Name)}={Uri.EscapeDataString(value.ToString())}&");
            }
        }

        // Remove the trailing '&'
        return queryString.ToString().TrimEnd('&');
    }
    private async Task<T> Send<T>(bool authenticate, HttpMethod method, string url, object data = null)
    {
        try
        {
            if (authenticate)
            {
                await EnsureAuthenticated();
            }
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
                _snackbarService.Add("BACKEND not enabled", Severity.Error, opt => opt.RequireInteraction = true);
                _snackbarService.Add("TURN ON THE BACK-END, \n Solution Explorer -> SmartHome.Backend -> r-click -> debug -> start new instance", Severity.Error, opt => opt.RequireInteraction = true);
                return default(T);
            }
#endif
            throw new Exception("Api error: " + ex.Message);
        }
    }
    private async Task AttachAuthorizationHeader(HttpRequestMessage request)
    {
        var jwt = await GetJwt();
        if (jwt is null)
            throw new Exception("User is not authenticated");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt.RawData);
    }

    private async Task EnsureAuthenticated()
    {
        var jwt = await GetJwt();

        if (jwt is null)
            throw new Exception("User is not authenticated");

        // Refresh if the token is expired
        if (DateTime.UtcNow >= jwt.ValidTo)
        {
            await Refresh();
        }
    }


    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient(_config.HttpClientName);
    }
    public async ValueTask<JwtSecurityToken?> GetJwt()
    {
        if (cachedJwt is null)
        { 
            string jwt = await _sessionStorageService.GetItemAsync<string>(JWT_KEY);
            if (!string.IsNullOrEmpty(jwt))
                cachedJwt = new JwtSecurityToken(jwt);
        }

        return cachedJwt;
    }
    private string GetUrl(string url)
    {
        return $"{_config.ApiBaseUrl}/{url}";
    }


    private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API Error: {response.StatusCode} {response.ReasonPhrase}");
        }
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Unable to parse Json");
    }

    //private static string PadBase64(string base64)
    //{
    //    return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
    //}
}

public class JwtPayload
{
    public long Exp { get; set; }
}
