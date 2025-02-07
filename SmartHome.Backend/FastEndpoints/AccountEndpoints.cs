using SmartHome.Backend.Auth;
using SmartHome.Backend.FastEndpoints.Base;
using SmartHome.Common;
using SmartHome.Common.Models.Auth;

namespace SmartHome.Backend.FastEndpoints;

public class ForgotPasswordEndpoint : AuthEndpointBase<ForgotPasswordRequest, GenericSuccessResponse>
{
    public override void Configure()
    {
        Post(SharedConfig.Urls.Account.ForgotPasswordUrl);
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgotPasswordRequest request, CancellationToken ct)
    {
        var user = UserManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            await SendAsync(GenericSuccessResponse.Failed($"User with email {request.Email} not found!"));
            return;
        }

        await SendAsync(GenericSuccessResponse.Success());
        return;
        //var user = new User()
        //{
        //    Email = request.EmailAddress,
        //    UserName = request.DisplayName,
        //    EmailConfirmed = true,
        //};

        //var result = await UserManager.CreateAsync(user, request.Password);
        //await SendAsync(new RegisterResponse());
    }
}
