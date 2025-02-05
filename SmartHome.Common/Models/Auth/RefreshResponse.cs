namespace SmartHome.Common.Models.Auth;

//????
public record RefreshResponse(bool Success, string? JWT = null)
{
    public static RefreshResponse Failed => new RefreshResponse(false, null);
}
