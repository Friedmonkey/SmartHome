using SmartHome.Common.Models.Entities;
using SmartHome.Database.ApiContext;
using SmartHome.Database.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartHome.Backend.Test.Testing;

public class TestAuthContext : IAuthContext
{
    private readonly AuthContext host;
    public TestAuthContext(AuthContext host)
    {
        this.host = host;
    }

    public ClaimsPrincipal? JWT => currentLogin;

    private ClaimsPrincipal currentLogin;
    public void Login(string jwtStr)
    {
        var jwt = new JwtSecurityToken(jwtStr);
        var identity = new ClaimsIdentity(jwt.Claims, "jwt");
        var user = new ClaimsPrincipal(identity);
        currentLogin = user;
    }

    public Task EnforceIsPartOfSmartHome(Guid smarthomeId) => host.EnforceIsPartOfSmartHome(smarthomeId);

    public Task EnforceIsSmartHomeAdmin(Guid smarthomeId) => host.EnforceIsSmartHomeAdmin(smarthomeId);

    public Task EnforceUserIsPartOfSmartHome(Guid smarthomeId, Guid smartUserId) => host.EnforceUserIsPartOfSmartHome(smarthomeId, smartUserId);
    public Task<AuthAccount> GetAccountByEmail(string email) => host.GetAccountByEmail(email);

    public Task<AuthAccount> GetAccountById(Guid id) => host.GetAccountById(id);

    public Task<AuthAccount> GetLoggedInAccount() => host.GetLoggedInAccount();

    public Guid GetLoggedInId() => host.GetLoggedInId();
    public Task<SmartUserModel> GetLoggedInSmartUser(Guid smarthomeId) => host.GetLoggedInSmartUser(smarthomeId);
    public Task<SmartUserModel> GetPendingInvite(Guid smarthomeId) => host.GetPendingInvite(smarthomeId);
    public Task<SmartUserModel?> GetSmartUser(Guid accountId, Guid smarthomeId) => host.GetSmartUser(accountId, smarthomeId);

    public Task<bool> IsSmartHomeAdmin(Guid smarthomeId) => host.IsSmartHomeAdmin(smarthomeId);
}
