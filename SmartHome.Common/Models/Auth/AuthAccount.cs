using Microsoft.AspNetCore.Identity;

namespace SmartHome.Database.Auth;

public class AuthAccount : IdentityUser<Guid>
{
}

//not used but required, so we just leave empty
public class Role : IdentityRole<Guid>
{
}