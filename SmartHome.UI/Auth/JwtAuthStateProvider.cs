using Microsoft.AspNetCore.Components.Authorization;
using SmartHome.Common;
using System.Security.Claims;

namespace SmartHome.UI.Auth;

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

    private static readonly AuthenticationState Unauthorized = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    private async Task<AuthenticationState> GetAuthenticationState()
    {
        var jwt = await _jwtStoreService.GetJwt();

        if (jwt is null)
            return Unauthorized;

        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        if (user.IsUser())
            return new AuthenticationState(user);

        Console.WriteLine("Invalid user ClaimsPrincipal!!");
        return Unauthorized;
    }
}
