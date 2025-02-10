using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SmartHome.Backend.Auth;
using SmartHome.Common.Api;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Backend.Api;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly BackendConfig _backendConfig;

    public AccountService(UserManager<User> userManager, SignInManager<User> signInManager, BackendConfig backendConfig)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _backendConfig = backendConfig;
    }

    public async Task<SuccessResponse> Register(RegisterRequest request)
    {
        var user = new User()
        {
            Email = request.Email,
            UserName = request.Username,
            EmailConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return SuccessResponse.Success();
        }
        else
        {
            var errors = result.Errors.Select(e => e.Description).ToList();
            return SuccessResponse.FailedJson(errors);
        }
    }
    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return TokenResponse.Failed($"User with email {request.Email} not found!");

        var signIn = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!signIn.Succeeded)
            return TokenResponse.Failed($"Email or Password is not correct.");

        return GetTokenResponse(user);
    }
    public async Task<TokenResponse> Refresh(TokenRequest request)
    {
        if (string.IsNullOrEmpty(request.Refresh))
            return TokenResponse.Failed("Refresh token was empty.");

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.SecurityStamp == request.Refresh);
        if (user is null)
            return TokenResponse.Failed("Refresh token was incorrect.");

        return GetTokenResponse(user);
    }
    public Task<SuccessResponse> Logout(EmptyRequest request)
    {
        return Task.FromResult(SuccessResponse.Failed("not implemented yet"));
    }
    public async Task<SuccessResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return SuccessResponse.Failed($"User with email {request.Email} not found!");

        return SuccessResponse.Failed("not implemented");
    }

    private TokenResponse GetTokenResponse(User user)
    {
        if (string.IsNullOrEmpty(user.SecurityStamp))
            throw new Exception("User Refresh key not set.");
        return new TokenResponse(JWT: CreateJWT(user), Refresh:user.SecurityStamp);
    }
    private string CreateJWT(User user)
    {
        var secretkey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_backendConfig.JwtKey));
        var credentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.UserName ?? throw new NoNullAllowedException("user.UserName")), // NOTE: this will be the "User.Identity.Name" value
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new NoNullAllowedException("user.Email")),
            new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        };

#warning change to 15 minutes

        var token = new JwtSecurityToken(
            issuer: _backendConfig.Domain,
            audience: _backendConfig.Domain,
            claims: claims,
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
