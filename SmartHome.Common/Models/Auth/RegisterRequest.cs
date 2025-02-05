namespace SmartHome.Common.Models.Auth;

public record RegisterRequest(string EmailAddress, string DisplayName, string Password)
{
    
}
