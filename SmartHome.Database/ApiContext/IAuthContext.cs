using SmartHome.Common.Models.Entities;
using SmartHome.Database.Auth;
using System.Security.Claims;

namespace SmartHome.Database.ApiContext
{
    public interface IAuthContext
    {
        ClaimsPrincipal? JWT { get; }

        Task EnforceIsPartOfSmartHome(Guid smarthomeId);
        Task EnforceIsSmartHomeAdmin(Guid smarthomeId);
        Task EnforceUserIsPartOfSmartHome(Guid smarthomeId, Guid smartUserId);
        Task<AuthAccount> GetAccountByEmail(string email);
        Task<AuthAccount> GetAccountById(Guid id);
        Task<AuthAccount> GetLoggedInAccount();
        Guid GetLoggedInId();
        Task<SmartUserModel> GetLoggedInSmartUser(Guid smarthomeId);
        Task<SmartUserModel> GetPendingInvite(Guid smarthomeId);
        Task<SmartUserModel?> GetSmartUser(Guid accountId, Guid smarthomeId);
        Task<bool> IsSmartHomeAdmin(Guid smarthomeId);
    }
}