namespace SmartHome.Common.Api;

public interface IAccountService
{
    public record RegisterRequest(string Email, string Username, string Password, string PasswordConfirm);
    public Task<EmptyResponse> Register(RegisterRequest request);

    public record LoginRequest(string Email, string Password);
    public Task<TokenResponse> Login(LoginRequest request);

    public record TokenRequest(string JWT, string Refresh);
    public Task<TokenResponse> Refresh(TokenRequest request);

    public Task<EmptyResponse> Logout(EmptyRequest request);

    public record ForgotPasswordRequest(string Email);
    public Task<EmptyResponse> ForgotPassword(ForgotPasswordRequest request);
}
