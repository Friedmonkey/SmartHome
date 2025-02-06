using SmartHome.Backend.Auth;
using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Models.Auth;

namespace SmartHome.Backend.FastEndpoints;

public class RegisterEndpoint : AuthEndpointBase<RegisterRequest, RegisterResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.RegisterUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken ct)
    {
        var user = new User()
        {
            Email = request.EmailAddress,
            UserName = request.DisplayName,
            EmailConfirmed = true,
        };

        var result = await UserManager.CreateAsync(user, request.Password);
        await SendAsync(new(Succeeded: true));
    }
}

public class LoginEndpoint : AuthEndpointBase<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.LoginUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var user = await UserManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var signIn = await SignInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (signIn.Succeeded)
            {
                string jwt = CreateJWT(user);
                AppendRefreshTokenCookie(user, HttpContext.Response.Cookies);

                await SendAsync(new LoginResponse(true, jwt));
                return;
            }
        }
        await SendAsync(LoginResponse.Failed);
    }
}

public class RefreshEndpoint : AuthEndpointBase<LoginRequest, LoginResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.RefreshUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var cookie = HttpContext.Request.Cookies[RefreshTokenCookieKey];
        if (cookie != null)
        {
            var user = UserManager.Users.FirstOrDefault(user => user.SecurityStamp == cookie);
            if (user != null)
            {
                var jwtToken = CreateJWT(user);
                await SendAsync(new LoginResponse(true, jwtToken));
                return;
            }
        }
        await SendAsync(LoginResponse.Failed);
    }
}