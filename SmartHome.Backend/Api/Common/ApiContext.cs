using Microsoft.AspNetCore.Identity;
using SmartHome.Database;
using SmartHome.Database.Auth;
using SmartHome.Database.ApiContext;

namespace SmartHome.Backend.Api.Common;

public class ApiContext
{
    private readonly UserManager<AuthAccount> _userManager;
    private readonly SignInManager<AuthAccount> _signInManager;
    private readonly BackendConfig _backendConfig;

    private readonly SmartHomeContext _dbContext;

    public readonly AuthContext Auth;
    public readonly DeviceContext Device;

    public ApiContext(
        AuthContext authCtx,
        DeviceContext deviceCtx,

        SmartHomeContext dbContext,
        UserManager<AuthAccount> userManager, SignInManager<AuthAccount> signInManager,
        BackendConfig backendConfig)
    {
        Auth = authCtx;
        Device = deviceCtx;

        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _backendConfig = backendConfig;
    }

    public SmartHomeContext DbContext => _dbContext;
    public UserManager<AuthAccount> UserManager => _userManager;
    public SignInManager<AuthAccount> SignInManager => _signInManager;
    public BackendConfig BackendConfig => _backendConfig;

    public Task SaveDatabase() => _dbContext.SaveChangesAsync();
}
