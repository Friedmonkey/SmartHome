using System.IdentityModel.Tokens.Jwt;

namespace SmartHome.UI.Auth;

public interface IJwtStoreService
{
    Task RemoveTokens();
    Task SetTokens(string jwt, string refresh);
    Task<string> GetRefreshToken();
    ValueTask<JwtSecurityToken?> GetJwt();
}
