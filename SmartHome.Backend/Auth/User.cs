using Microsoft.AspNetCore.Identity;

namespace SmartHome.Backend.Auth;

public class User : IdentityUser<long>
{
}

public class Role : IdentityRole<long>
{
}