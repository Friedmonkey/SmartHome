using SmartHome.Backend.Auth;
using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Models.Auth;

namespace SmartHome.Backend.FastEndpoints;

public class RegisterEndpoint : AuthEndpointBase<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.AuthUrls.RegisterUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken ct)
    {
        var user = new User()
        {
            Email = request.Email,
            UserName = request.Username,
            EmailConfirmed = true,
        };

        var result = await UserManager.CreateAsync(user, request.Password);
        await SendAsync(new RegisterResponse());
    }
}

public class LoginEndpoint : AuthEndpointBase<LoginRequest, TokenResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.AuthUrls.LoginUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await UserManager.FindByEmailAsync(request.Email);
        if (user is null)
        { 
            await SendAsync(TokenResponse.Failed($"User with email {request.Email} not found!"));
            return;
        }
        var signIn = await SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!signIn.Succeeded)
        {
            await SendAsync(TokenResponse.Failed($"Email or Password is not correct."));
            return;
        }

        string jwt = CreateJWT(user);
        AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

        await SendAsync(new TokenResponse(JWT:jwt));
    }
}

public class RefreshEndpoint : AuthEndpointBase<RefreshRequest, TokenResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.AuthUrls.RefreshUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RefreshRequest request, CancellationToken ct)
    {
        var cookie = HttpContext.Request.Cookies[RefreshTokenCookieKey];
        if (cookie is null)
        { 
            await SendAsync(TokenResponse.Failed("refresh cookie was empty"));
            return;
        }

        var user = UserManager.Users.FirstOrDefault(user => user.SecurityStamp == cookie);
        if (user is null)
        { 
            await SendAsync(TokenResponse.Failed($"User cookie not found!"));
            return;
        }

        var jwtToken = CreateJWT(user);
        await SendAsync(new TokenResponse(JWT:jwtToken));
    }
}