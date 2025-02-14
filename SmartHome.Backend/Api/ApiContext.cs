using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using SmartHome.Database;
using SmartHome.Database.Auth;
using System.Security.Claims;

namespace SmartHome.Backend.Api;

public class ApiContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly SmartHomeContext _dbContext;
    private readonly UserManager<AuthAccount> _userManager;
    private readonly SignInManager<AuthAccount> _signInManager;
    private readonly BackendConfig _backendConfig;

    public ApiContext(
        IHttpContextAccessor contextAccessor, SmartHomeContext dbContext, 
        UserManager<AuthAccount> userManager, SignInManager<AuthAccount> signInManager, 
        BackendConfig backendConfig)
    { 
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _backendConfig = backendConfig;
    }

    public ClaimsPrincipal? JWT => _contextAccessor?.HttpContext?.User;
    public SmartHomeContext DbContext => _dbContext;
    public UserManager<AuthAccount> UserManager => _userManager;
    public SignInManager<AuthAccount> SignInManager => _signInManager;
    public BackendConfig BackendConfig => _backendConfig;

    public async Task EnforceIsSmartHomeAdmin(Guid smarthomeId)
    {
        if (!(await IsSmartHomeAdmin(smarthomeId)))
            throw new ApiError("You do not have access to control this SmartHome");
    }
    public async Task<bool> IsSmartHomeAdmin(Guid smarthomeId)
    {
        var smartUser = await GetLoggedInSmartUser(smarthomeId);
        bool isAdmin = (smartUser.Role == UserRole.Admin);
        return isAdmin;
    }
    public async Task<SmartUserModel> GetLoggedInSmartUser(Guid smarthomeId)
    {
        var id = GetLoggedInId();
        var smartUser = await GetSmartUser(id, smarthomeId);
        if (smartUser is null)
            throw new ApiError("You are not part of this SmartHome!");
        return smartUser;
    }
    public async Task<SmartUserModel?> GetSmartUser(Guid accountId, Guid smarthomeId)
    {
        var smartUser = await DbContext.SmartUsers.FirstOrDefaultAsync(sm => sm.AccountId == accountId && sm.SmartHomeId == smarthomeId);
        return smartUser;
    }
    public Task<AuthAccount> GetLoggedInAccount()
    {
        return GetAccountById(GetLoggedInId());
    }
    public Guid GetLoggedInId()
    {
        if (JWT?.IsUser() is false or null)
            throw new ApiError("Unable to resolve login!");
        var id = JWT.GetID();
        return id;
    }
    public async Task<AuthAccount> GetAccountById(Guid id)
    {
        var account = await _userManager.Users.FirstOrDefaultAsync(aa => aa.Id == id);
        if (account is null)
            throw new ApiError($"Unable to get account with id: {id}");
        return account;
    }
    public async Task<AuthAccount> GetAccountByEmail(string email)
    {
        var account = await _userManager.FindByEmailAsync(email);
        if (account is null)
            throw new ApiError($"Account with email: {email} does not exist!");
        return account;
    }
}
