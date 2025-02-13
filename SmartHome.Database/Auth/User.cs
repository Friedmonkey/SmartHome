using Microsoft.AspNetCore.Identity;

namespace SmartHome.Database.Auth;

public class User : IdentityUser<long>
{
}

//not used but required, so we just leave empty
public class Role : IdentityRole<long>
{
}