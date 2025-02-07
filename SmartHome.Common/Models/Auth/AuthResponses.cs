namespace SmartHome.Common.Models.Auth;

public record TokenResponse(string? JWT = null) : Response<TokenResponse>;

//public record LoginResponse(string? JWT = null, string? RefreshToken = null) : Response<LoginResponse>;
//public record RefreshResponse(string? JWT = null, string? RefreshToken = null) : Response<RefreshResponse>;
public record RegisterResponse() : Response<RegisterResponse>;
