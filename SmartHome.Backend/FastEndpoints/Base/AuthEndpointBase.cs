using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SmartHome.Backend.Auth;
using SmartHome.Common.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartHome.Backend.FastEndpoints.Base;

public abstract class AuthEndpointBase<TRequest, TResponse>: Endpoint<TRequest, TResponse>
    where TRequest: notnull
    where TResponse: Response<TResponse>
{
    //dependency injection
    public required UserManager<User> UserManager { get; set; }
    public required SignInManager<User> SignInManager { get; set; }
    public required BackendConfig BackendConfig { get; set; }


    public const string RefreshTokenCookieKey = "refresh_token";
    public void AppendRefreshTokenCookie(User user, IResponseCookies cookies)
    {
        var options = new CookieOptions();
        options.HttpOnly = true;
        options.Secure = true;
        options.SameSite = SameSiteMode.Strict;
        options.Expires = DateTime.Now.AddMinutes(60);
        cookies.Append(RefreshTokenCookieKey, user.SecurityStamp, options);
    }

    public string CreateJWT(User user)
    {
        var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(BackendConfig.JwtKey));
        var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName), // NOTE: this will be the "User.Identity.Name" value
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };

#warning change to 15 minutes

        var token = new JwtSecurityToken(
            issuer: BackendConfig.Domain,
            audience: BackendConfig.Domain,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
