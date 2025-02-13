using JwtBearer = FastEndpoints.Security.JwtBearer;
using static SmartHome.Common.Api.IAccountService;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using SmartHome.Database.Auth;

namespace SmartHome.Backend.Api;

public class AccountService : IAccountService
{
    private readonly ApiContext _ctx;

    public AccountService(ApiContext apiContext)
    {
        _ctx = apiContext;
    }

    public async Task<SuccessResponse> Register(RegisterRequest request)
    {
        if (request.Password != request.PasswordConfirm)
            return SuccessResponse.Failed("Passwords do not match");
        var user = new AuthAccount()
        {
            Email = request.Email,
            UserName = request.Username,
            EmailConfirmed = true,
        };

        var result = await _ctx.UserManager.CreateAsync(user, request.Password);
        return GetSuccessResponse(result);
    }
    public async Task<TokenResponse> Login(LoginRequest request)
    {
        var user = await _ctx.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
            return TokenResponse.Failed($"User with email {request.Email} not found!");

        var signIn = await _ctx.SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!signIn.Succeeded)
            return TokenResponse.Failed($"Email or Password is not correct.");

        return await GetTokenResponse(user);
    }
    public async Task<SuccessResponse> Logout(EmptyRequest request)
    {
        var user = await _ctx.GetLoggedInAccount();
        var result = await _ctx.UserManager.UpdateSecurityStampAsync(user);
        return GetSuccessResponse(result);
    }
    public async Task<TokenResponse> Refresh(RefreshRequest request)
    {
        if (string.IsNullOrEmpty(request.Refresh))
            return TokenResponse.Failed("Refresh token was empty.");

        var user = await _ctx.UserManager.Users.FirstOrDefaultAsync(user => user.SecurityStamp == request.Refresh);
        if (user is null)
            return TokenResponse.Failed("Refresh token was incorrect.");

        return await GetTokenResponse(user);
    }
    public async Task<SuccessResponse> ForgotPassword(ForgotPasswordRequest request)
    {
        var user = await _ctx.UserManager.FindByEmailAsync(request.Email);
        if (user is null)
            return SuccessResponse.Failed($"User with email {request.Email} not found!");

        return SuccessResponse.Failed("not implemented");
    }

    private async Task<TokenResponse> GetTokenResponse(AuthAccount user)
    {
        if (string.IsNullOrEmpty(user.SecurityStamp))
            return TokenResponse.Failed("User Refresh key not set.");

        var result = await _ctx.UserManager.UpdateSecurityStampAsync(user);
        if (!result.Succeeded)
            return TokenResponse.Failed("User Refresh failed.");

        return new TokenResponse(JWT: CreateJWT(user), Refresh:user.SecurityStamp);
    }
    private SuccessResponse GetSuccessResponse(IdentityResult? result) 
    {
        if (result is null)
            return SuccessResponse.Failed("IdentityResult was null");
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
    private string CreateJWT(AuthAccount user)
    {
#warning change to 15 minutes

        var jwtToken = JwtBearer.CreateToken(o =>
        {
            o.SigningKey = _ctx.BackendConfig.JwtSecret;
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()));
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? throw new NoNullAllowedException("user.UserName")));
            o.User.Claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new NoNullAllowedException("user.Email")));
            o.User.Roles.Add(AuthRoles.AuthUser);
            o.ExpireAt = DateTime.Now.AddMinutes(1);
        });

        return jwtToken;
    }
}
