using Blazored.SessionStorage;
using System.IdentityModel.Tokens.Jwt;

namespace SmartHome.UI.Auth;

public class JwtSessionStorageService : IJwtStoreService
{
    private readonly ISessionStorageService _sessionStorageService;

    private const string JWT_KEY = "jwt_token";
    private const string REFRESH_KEY = "refresh_token";
    private JwtSecurityToken? cachedJwt = null;

    public JwtSessionStorageService(ISessionStorageService sessionStorage)
    {
        _sessionStorageService = sessionStorage;
    }

    public async Task RemoveTokens()
    {
        await _sessionStorageService.RemoveItemAsync(JWT_KEY);
        await _sessionStorageService.RemoveItemAsync(REFRESH_KEY);

        cachedJwt = null;
    }

    public async Task SetTokens(string jwt, string refresh)
    {
        await _sessionStorageService.SetItemAsync(JWT_KEY, jwt);
        await _sessionStorageService.SetItemAsync(REFRESH_KEY, refresh);
        cachedJwt = new JwtSecurityToken(jwt);
    }

    public async Task<string> GetRefreshToken()
    {
        return await _sessionStorageService.GetItemAsync<string>(REFRESH_KEY);
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
}
