namespace SmartHome.Common.Api;

public interface IAccountService
{
    public record RegisterRequest(string Email, string Username, string Password, string PasswordConfirm);
    public Task<EmptyResponse> Register(RegisterRequest request);

    public record LoginRequest(string Email, string Password);
    public Task<TokenResponse> Login(LoginRequest request); 

    public Task<TokenResponse> Refresh(EmptyRequest request);

    public record ForgotPasswordRequest(string Email);
    public Task<EmptyResponse> ForgotPassword(ForgotPasswordRequest request);
}
