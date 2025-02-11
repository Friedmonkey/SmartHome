using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace SmartHome.UI.Api;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private readonly IJwtStoreService _jwtStoreService;
    public JwtAuthStateProvider(IJwtStoreService jwtStoreService)
    {
        _jwtStoreService = jwtStoreService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return await GetAuthenticationState();
    }

    public void UpdateAuthState()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationState());
    }

    private async Task<AuthenticationState> GetAuthenticationState()
    {
        var jwt = await _jwtStoreService.GetJwt();
        var identity = new ClaimsIdentity();

        if (jwt is not null)
        {
            identity = new ClaimsIdentity(jwt!.Claims, "jwt");
        }

        var user = new ClaimsPrincipal(identity);
        return new AuthenticationState(user);
    }
}
