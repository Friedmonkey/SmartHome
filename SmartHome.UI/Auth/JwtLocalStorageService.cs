using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;

namespace SmartHome.UI.Auth;

public class JwtLocalStorageService : IJwtStoreService
{
    private readonly ILocalStorageService _localStorageService;

    private const string JWT_KEY = "jwt_token";
    private const string REFRESH_KEY = "refresh_token";
    private JwtSecurityToken? cachedJwt = null;

    public JwtLocalStorageService(ILocalStorageService localStorage)
    {
        _localStorageService = localStorage;
    }

    public async Task RemoveTokens()
    {
        await _localStorageService.RemoveItemAsync(JWT_KEY);
        await _localStorageService.RemoveItemAsync(REFRESH_KEY);

        cachedJwt = null;
    }

    public async Task SetTokens(string jwt, string refresh)
    {
        await _localStorageService.SetItemAsync(JWT_KEY, jwt);
        await _localStorageService.SetItemAsync(REFRESH_KEY, refresh);
        cachedJwt = new JwtSecurityToken(jwt);
    }

    public async Task<string> GetRefreshToken()
    {
        return await _localStorageService.GetItemAsync<string>(REFRESH_KEY);
    }

    public async ValueTask<JwtSecurityToken?> GetJwt()
    {
        if (cachedJwt is null)
        {
            string jwt = await _localStorageService.GetItemAsync<string>(JWT_KEY);
            if (!string.IsNullOrEmpty(jwt))
                cachedJwt = new JwtSecurityToken(jwt);
        }

        return cachedJwt;
    }
}
