namespace SmartHome.Common.Models.Auth;

public record LoginRequest(string Email, string Password);
public record RefreshRequest(string RefreshToken);
public record RegisterRequest(string EmailAddress, string DisplayName, string Password);
