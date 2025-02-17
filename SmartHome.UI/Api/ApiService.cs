using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using MudBlazor;
using SmartHome.Common;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using SmartHome.UI.Auth;
using SmartHome.UI.Layout;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.UI.Api;

public class ApiService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ISnackbar _snackbarService;
    private readonly IJwtStoreService _jwtStoreService;
    private readonly SmartHomeState _smartHomeState;
    private readonly JwtAuthStateProvider _jwtAuthStateProvider;
    private readonly FrontendConfig _config;

    public ApiService(IHttpClientFactory httpClientFactory, ISnackbar snackbarService, FrontendConfig config, IJwtStoreService jwtStoreService, JwtAuthStateProvider jwtAuthStateProvider, SmartHomeState smartHomeState)
    {
        _httpClientFactory = httpClientFactory;
        _snackbarService = snackbarService;
        _config = config;
        _jwtStoreService = jwtStoreService;
        _jwtAuthStateProvider = jwtAuthStateProvider;
        _smartHomeState = smartHomeState;
    }

    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var response = await Post<TokenResponse>(SharedConfig.Urls.Account.LoginUrl, request, authenticated: false);

        if (response.WasSuccess())
        {
            await _jwtStoreService.SetTokens(response.JWT, response.Refresh);
            _jwtAuthStateProvider.UpdateAuthState();
        }
        return response;
    }
    public async Task Logout()
    {
        var response = await Delete<SuccessResponse>(SharedConfig.Urls.Account.LogoutUrl);
        response.Show(_snackbarService, "Logout success!");
        await _jwtStoreService.RemoveTokens();
        _jwtAuthStateProvider.UpdateAuthState();
    }
    public async Task<TokenResponse> Refresh()
    {
        var refresh = await _jwtStoreService.GetRefreshToken();

        var request = new RefreshRequest(Refresh: refresh);
        var response = await Post<TokenResponse>(SharedConfig.Urls.Account.RefreshUrl, request, authenticated: false);

        response.EnforceSuccess();
        await _jwtStoreService.SetTokens(response.JWT, response.Refresh);

        return response;
    }
    public async Task<T> Get<T>(string url, object? data = null, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Get, url, data);
    }
    public async Task<T> Post<T>(string url, object? data, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Post, url, data);
    }
    public async Task<T> Put<T>(string url, object? data = null, bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Put, url, data);
    }
    public async Task<T> Delete<T>(string url, object? data = null,  bool authenticated = true) where T : Response<T>
    {
        return await Send<T>(authenticated, HttpMethod.Delete, url, data);
    }
    private async Task<T> Send<T>(bool authenticate, HttpMethod method, string url, object? data = null) where T : Response<T>
    {
        try
        {
            JsonContent? content = null;
            AuthenticationHeaderValue? authHeader = null;

            if (authenticate)
                authHeader = await GetAuthHeader();

            if (data is not null)
            {
                if (data is SmartHomeRequest req)
                {
                    if (!_smartHomeState.SelectedSmartHomeId.HasValue)
                        throw new ApiError("Unable to resolve SmartHome Guid from state.", fatal:true);
                    if (req.smartHome == Guid.Empty)
                        _snackbarService.Add("Overriding SmartHome Guid", Severity.Warning);

                    req.UpdateSmartHome((Guid)_smartHomeState.SelectedSmartHomeId);
                }
                content = GetData(method, data, ref url); //can put data in url for GET requests
            }

            var newUrl = GetUrl(url);
            var request = new HttpRequestMessage(method, newUrl);
            request.Content = content;
            request.Headers.Authorization = authHeader;

            var response = await HandleResponse<T>(await GetHttpClient().SendAsync(request));
            return response;
        }
        catch (ApiError apiError) when (apiError.IsFatal == false)
        { 
            return Response<T>.Failed(apiError.Message);
        }
        catch (Exception ex) when (ex is not ApiError)
        {
#if DEBUG
            if (ex.Message.StartsWith("TypeError: Failed to fetch"))
            {
                _snackbarService.Add("TURN ON THE BACK-END, \n Solution Explorer -> SmartHome.Backend -> r-click -> debug -> start new instance", Severity.Error, opt => opt.RequireInteraction = true);
                return Response<T>.Failed("BACKEND not enabled");
            }
#endif
            throw new ApiError("Unexpected Api error: " + ex.Message, fatal:true);
        }
    }
    private JsonContent? GetData(HttpMethod method, object data, ref string url)
    {
        if (method == HttpMethod.Post)
            return JsonContent.Create(data);

        if (method == HttpMethod.Get)
            url = ParseDataAsQuery(url, data);
        else if (method == HttpMethod.Put)
            url = ParseDataAsQuery(url, data);
        else if (method == HttpMethod.Delete)
            url = ParseDataAsQuery(url, data);
        else throw new NotSupportedException($"Http method: {method.Method} is not supported!");

        return null;
    }
    private async Task<AuthenticationHeaderValue> GetAuthHeader()
    {
        var jwt = await _jwtStoreService.GetJwt();
        if (jwt is null)
            throw new ApiError("User is not authenticated.");

        // Refresh if the token is expired
        if (DateTime.UtcNow >= jwt.ValidTo)
        {
            await Refresh();
            jwt = await _jwtStoreService.GetJwt();
        }

        return new AuthenticationHeaderValue("Bearer", jwt!.RawData);
    }
    private string ParseDataAsQuery(string url, object data)
    {
        var queryString = new StringBuilder();

        queryString.Append(url);
        queryString.Append('?');

        // Serialize the object into key-value pairs for query params
        foreach (var property in data.GetType().GetProperties())
        {
            var value = property.GetValue(data);
            if (value is not null)
            {
                var val = value.ToString() ?? string.Empty;
                queryString.Append(Uri.EscapeDataString(property.Name));
                queryString.Append('=');
                queryString.Append(Uri.EscapeDataString(val));
                queryString.Append('&');
            }
        }

        // Remove the trailing '&' or '?'
        return queryString.ToString().TrimEnd('&').TrimEnd('?');
    }

    private HttpClient GetHttpClient()
    {
        return _httpClientFactory.CreateClient(_config.HttpClientName);
    }

    private string GetUrl(string url)
    {
        return $"{_config.ApiBaseUrl}/{url}";
    }


    private static async Task<T> HandleResponse<T>(HttpResponseMessage response) where T : Response<T>
    {
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Response<T>.Failed("Authentication failed.");
            }
            throw new Exception($"API Error: {response.StatusCode} {response.ReasonPhrase}");
        }
        return await response.Content.ReadFromJsonAsync<T>() ?? throw new Exception("Unable to parse Json");
    }
}
