namespace SmartHome.Common.Models.Auth;

public record LoginResponse(bool Success, string? JWT = null)
{
    public static LoginResponse Failed => new LoginResponse(false, null);
}
