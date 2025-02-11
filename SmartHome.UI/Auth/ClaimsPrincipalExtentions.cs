using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SmartHome.UI.Auth;

public static class ClaimsPrincipalExtensions
{
    public static bool IsUser(this ClaimsPrincipal claimsPrincipal)
    {
        var userClaims = new List<string>() 
        {
            JwtRegisteredClaimNames.Name,
            JwtRegisteredClaimNames.Email,
        };
        return HasAllClaims(claimsPrincipal, userClaims);
    }

    public static bool HasAllClaims(this ClaimsPrincipal claimsPrincipal, List<string> requiredClaims)
    {
        return requiredClaims.All(claim => claimsPrincipal.HasClaim(c => c.Type == claim));
    }

    public static string? GetClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return claimsPrincipal.FindFirst(claimType)?.Value;
    }

    public static string GetRequiredClaimValue(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        return GetClaimValue(claimsPrincipal, claimType) ?? throw new ArgumentNullException(claimType);
    }

    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        return GetRequiredClaimValue(claimsPrincipal, "name");
    }

    public static string? GetProfilePicture(this ClaimsPrincipal claimsPrincipal)
    {
        return GetClaimValue(claimsPrincipal, "profile_pic");
    }

    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return GetRequiredClaimValue(claimsPrincipal, "email");
    }
}

