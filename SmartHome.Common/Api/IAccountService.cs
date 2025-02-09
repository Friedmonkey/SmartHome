using SmartHome.Common.Models;

namespace SmartHome.Common.Api;


public record TokenResponse(string JWT, string Refresh) : Response<TokenResponse>;
public interface IAccountService
{
    public record RegisterRequest(string Email, string Username, string Password, string PasswordConfirm);
    public Task<SuccessResponse> Register(RegisterRequest request);

    public record LoginRequest(string Email, string Password);
    public Task<TokenResponse> Login(LoginRequest request);

    public record TokenRequest(string JWT, string Refresh);
    public Task<TokenResponse> Refresh(TokenRequest request);

    public Task<SuccessResponse> Logout(EmptyRequest request);

    public record ForgotPasswordRequest(string Email);
    public Task<SuccessResponse> ForgotPassword(ForgotPasswordRequest request);
}
