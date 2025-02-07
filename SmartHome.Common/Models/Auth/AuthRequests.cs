namespace SmartHome.Common.Models.Auth;

public record LoginRequest(string Email, string Password);
public record RefreshRequest();
public record RegisterRequest(string Email, string Username, string Password, string PasswordConfirm);
