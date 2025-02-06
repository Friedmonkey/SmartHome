using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using SmartHome.Backend.Auth;
using SmartHome.Common;
using SmartHome.Common.Models.Auth;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace SmartHome.Backend.Controllers;

[ApiController]
public class AuthController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly BackendConfig _config;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, BackendConfig config)
    {
        this._userManager = userManager;
        this._signInManager = signInManager;
        _config = config;
    }


    [HttpPost(SharedConfig.RegisterUrl)]
    public async Task<RegisterResponse> Register(RegisterRequest request)
    {
        var user = new User()
        {
            Email = request.EmailAddress,
            UserName = request.DisplayName,
            EmailConfirmed = true,
        };

        var result = await this._userManager.CreateAsync(user, request.Password);
        return new RegisterResponse(result.Succeeded);
    }

    [HttpPost(SharedConfig.LoginUrl)]
    public async Task<LoginResponse> Login(LoginRequest request)
    {
        var user = await this._userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var signIn = await this._signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signIn.Succeeded)
            {
                string jwt = CreateJWT(user);
                AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

                return new LoginResponse(true, jwt);
            }
            else
            {
                return LoginResponse.Failed;
            }
        }
        else
        {
            return LoginResponse.Failed;
        }
    }

    [HttpPost(SharedConfig.RefreshUrl)]
    public LoginResponse RefreshToken()
    {
        var cookie = HttpContext.Request.Cookies[RefreshTokenCookieKey];
        if (cookie != null)
        {
            var user = this._userManager.Users.FirstOrDefault(user => user.SecurityStamp == cookie);
            if (user != null)
            {
                var jwtToken = CreateJWT(user);
                return new LoginResponse(true, jwtToken);
            }
            else
            {
                return LoginResponse.Failed;
            }
        }
        else
        {
            return LoginResponse.Failed;
        }
    }

    private const string RefreshTokenCookieKey = "refresh_token";
    private void AppendRefreshTokenCookie(User user, IResponseCookies cookies)
    {
        var options = new CookieOptions();
        options.HttpOnly = true;
        options.Secure = true;
        options.SameSite = SameSiteMode.Strict;
        options.Expires = DateTime.Now.AddMinutes(60);
        cookies.Append(RefreshTokenCookieKey, user.SecurityStamp, options);
    }

    private string CreateJWT(User user)
    {
        var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.JwtKey));
        var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName), // NOTE: this will be the "User.Identity.Name" value
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _config.Domain,
            audience: _config.Domain,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
