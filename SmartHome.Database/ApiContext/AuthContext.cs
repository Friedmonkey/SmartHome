using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using SmartHome.Common;
using SmartHome.Database.Auth;
using System.Security.Claims;

namespace SmartHome.Database.ApiContext;

public class AuthContext
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly SmartHomeContext _dbContext;
    private readonly UserManager<AuthAccount> _userManager;

    public AuthContext(
    IHttpContextAccessor contextAccessor, SmartHomeContext dbContext,
    UserManager<AuthAccount> userManager)
    {
        _contextAccessor = contextAccessor;
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public ClaimsPrincipal? JWT => _contextAccessor?.HttpContext?.User;

    public async Task EnforceIsSmartHomeAdmin(Guid smarthomeId)
    {
        if (!await IsSmartHomeAdmin(smarthomeId))
            throw new ApiError("You do not have access to control this SmartHome");
    }
    public async Task EnforceIsPartOfSmartHome(Guid smarthomeId)
    {
        var account = GetLoggedInId();
        var isPartOfSmartHome = await _dbContext.SmartUsers.AnyAsync(sm => sm.AccountId == account && sm.SmartHomeId == smarthomeId);

        if (!isPartOfSmartHome)
            throw new ApiError("You are not part of this SmartHome");
    }
    public async Task<bool> IsSmartHomeAdmin(Guid smarthomeId)
    {
        var smartUser = await GetLoggedInSmartUser(smarthomeId);
        bool isAdmin = smartUser.Role == UserRole.Admin;
        return isAdmin;
    }
#warning TODO: make sure GetLoggedInSmartUser is user or admin and make separate one for invites
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
        var smartUser = await _dbContext.SmartUsers.FirstOrDefaultAsync(sm => sm.AccountId == accountId && sm.SmartHomeId == smarthomeId);
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
