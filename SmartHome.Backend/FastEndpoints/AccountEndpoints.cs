using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Backend.FastEndpoints;

public class RegisterEndpoint : BasicEndpointBase<RegisterRequest, EmptyResponse>
{
    public required IAccountService AccountService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Account.RegisterUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest request, CancellationToken ct)
    {
        await SendAsync(await AccountService.Register(request));
    }
}

public class LoginEndpoint : BasicEndpointBase<LoginRequest, TokenResponse>
{
    public required IAccountService AccountService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Account.LoginUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest request, CancellationToken ct)
    {
        await SendAsync(await AccountService.Login(request));
    }
}

public class RefreshEndpoint : BasicEndpointBase<TokenRequest, TokenResponse>
{
    public required IAccountService AccountService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Account.RefreshUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(TokenRequest request, CancellationToken ct)
    {
        await SendAsync(await AccountService.Refresh(request));
    }
}

public class ForgotPasswordEndpoint : BasicEndpointBase<ForgotPasswordRequest, EmptyResponse>
{
    public required IAccountService AccountService { get; set; }
    public override void Configure()
    {
        Post(SharedConfig.Urls.Account.ForgotPasswordUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        await SendAsync(await AccountService.ForgotPassword(request));
    }
}