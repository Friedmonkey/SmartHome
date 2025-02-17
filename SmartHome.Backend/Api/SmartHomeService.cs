using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.Backend.Api
{
    public class SmartHomeService : ISmartHomeService
    {
        private readonly ApiContext _ctx;

        public SmartHomeService(ApiContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<GuidResponse> CreateSmartHome(CreateSmartHomeRequest request)
        {
            var home = new SmartHomeModel()
            {
                Id = Guid.NewGuid(),
                Name = request.name,
                SSID = request.wifiname,
                SSPassword = request.password,
            };
            var homeResult = await _ctx.DbContext.SmartHomes.AddAsync(home);
            var smartUser = new SmartUserModel()
            { 
                AccountId = _ctx.GetLoggedInId(),
                SmartHomeId = homeResult.Entity.Id,
                Role = UserRole.Admin,
            };
            await _ctx.DbContext.SmartUsers.AddAsync(smartUser);

            await _ctx.DbContext.SaveChangesAsync();

            return new GuidResponse(homeResult.Entity.Id);
        }
        public async Task<SuccessResponse> InviteToSmartHome(InviteRequest request)
        {
            await _ctx.EnforceIsSmartHomeAdmin(request.smartHome);

            var acc = await _ctx.GetAccountByEmail(request.email);

            var smartUser = new SmartUserModel() 
            {
                AccountId = acc.Id,
                SmartHomeId = request.smartHome,
                Role = UserRole.InvitationPending,
            };

            var result = await _ctx.DbContext.SmartUsers.AddAsync(smartUser);
            if (result is null)
                return SuccessResponse.Failed("Failed to send invitation");
            return SuccessResponse.Success();
        }
        public async Task<SuccessResponse> AcceptSmartHomeInvite(SmartHomeRequest request)
        {
            var smartUser = await _ctx.GetLoggedInSmartUser(request.smartHome);

            if (smartUser.Role != UserRole.InvitationPending)
                return SuccessResponse.Failed("You dont have an invitation.");

            smartUser.Role = UserRole.User;

            await _ctx.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
        public async Task<SmartHomeListResponse> GetJoinedSmartHomes(EmptyRequest request)
        {
            var smartHomeIds = GetSmartUsers()
                .Where(su => su.Role == UserRole.Admin || su.Role == UserRole.User || su.Role == UserRole.Guest)
                .Select(su => su.SmartHomeId);

            return await GetHomesFromIds(smartHomeIds);
        }
        public async Task<SmartHomeListResponse> GetSmartHomeInvites(EmptyRequest request)
        {
            var smartHomeIds = GetSmartUsers()
                .Where(su => su.Role == UserRole.InvitationPending)
                .Select(su => su.SmartHomeId);

            return await GetHomesFromIds(smartHomeIds);
        }
        public async Task<SmartHomeResponse> GetSmartHomeById(GuidRequest request)
        {
            await _ctx.GetLoggedInSmartUser(request.Id); //make sure we are apart of the smarthome

            var smartHome = await _ctx.DbContext.SmartHomes.FirstOrDefaultAsync(home => home.Id == request.Id);
            if (smartHome is null)
                return SmartHomeResponse.Failed("Smart home not found");
            return new SmartHomeResponse(smartHome);
        }

        public async Task<SmartHomeListResponse> GetHomesFromIds(IQueryable<Guid> ids)
        {
             var smartHomes = await _ctx.DbContext.SmartHomes
                    .Where(home => ids.Contains(home.Id))
                    .ToListAsync();

            return new SmartHomeListResponse(smartHomes);
        }


        private IQueryable<SmartUserModel> GetSmartUsers()
        {
            var userId = _ctx.GetLoggedInId();
            var smartUsers = _ctx.DbContext.SmartUsers.Where(su => su.AccountId == userId);
            return smartUsers;
        }
    }
}
