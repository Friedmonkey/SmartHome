using JwtBearer = FastEndpoints.Security.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Auth;
using SmartHome.Common.Api;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static SmartHome.Common.Api.IAccountService;
using Microsoft.IdentityModel.Tokens;

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
        if (request.Password != request.PasswordConfirm)
            return SuccessResponse.Failed("Passwords do not match");
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

        return await GetTokenResponse(user);
    }
    public async Task<TokenResponse> Refresh(RefreshRequest request)
    {
        if (string.IsNullOrEmpty(request.Refresh))
            return TokenResponse.Failed("Refresh token was empty.");

        var user = await _userManager.Users.FirstOrDefaultAsync(user => user.SecurityStamp == request.Refresh);
        if (user is null)
            return TokenResponse.Failed("Refresh token was incorrect.");

        return await GetTokenResponse(user);
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

    private async Task<TokenResponse> GetTokenResponse(User user)
    {
        if (string.IsNullOrEmpty(user.SecurityStamp))
            return TokenResponse.Failed("User Refresh key not set.");

        var result = await _userManager.UpdateSecurityStampAsync(user);
        if (!result.Succeeded)
            return TokenResponse.Failed("User Refresh failed.");

        return new TokenResponse(JWT: CreateJWT(user), Refresh:user.SecurityStamp);
    }
    private string CreateJWT(User user)
    {
        //var claims = new[]
        //{
        //    new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? throw new NoNullAllowedException("user.UserName")), // NOTE: this will be the "User.Identity.Name" value
        //    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new NoNullAllowedException("user.Email")),
        //    new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
        //    //new Claim("role", AuthRoles.AuthUser), //Fastendpoint looks for 'role' by default
        //};

#warning change to 15 minutes

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            //o.SigningAlgorithm = SecurityAlgorithms.HmacSha256;

            o.SigningKey = _backendConfig.JwtSecret;
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? throw new NoNullAllowedException("user.UserName")));
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new NoNullAllowedException("user.Email")));
            o.User.Roles.Add(AuthRoles.AuthUser);
            o.ExpireAt = DateTime.Now.AddMinutes(1);
        });

        return jwtToken;
    }
}
