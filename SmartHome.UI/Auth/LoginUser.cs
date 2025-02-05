using System.Security.Claims;

namespace SmartHome.UI.Auth;

class LoginUser
{

    public LoginUser(string name, string jwt, ClaimsPrincipal claimsPrincipal)
    {
        this.DisplayName = name;
        this.Jwt = jwt;
        this.ClaimsPrincipal = claimsPrincipal;
    }

    public string DisplayName { get; set; }
    public string Jwt { get; set; }
    public ClaimsPrincipal ClaimsPrincipal { get; set; }
}
