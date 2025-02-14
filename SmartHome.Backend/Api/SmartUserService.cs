using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.ISmartUserService;

namespace SmartUser.Backend.Api;

public class SmartUserService : ISmartUserService
{
    private readonly ApiContext _ctx;

    public SmartUserService(ApiContext apiContext)
    {
        _ctx = apiContext;
    }

    public async Task<SuccessResponse> Create(CreateRequest request)
    {
        try
        {
            var result = await _ctx.DbContext.SmartUsers.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                Role = request.RoleId,
                SmartHomeId = request.SmartHomeId
            });
            await _ctx.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
           return  SuccessResponse.Error(ex);
        }
    }

    public async Task<SuccessResponse> Delete(GuidRequest request)
    {
        try
        {
            var result = await _ctx.DbContext.SmartUsers.FindAsync(request.Id);
            if (result == null) {
                return SuccessResponse.Failed("Smart User not found");
            }
            _ctx.DbContext.SmartUsers.Remove(result);
            await _ctx.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.Error(ex);
        }
    }

    public async Task<SmartUserResponse> GetSmartUsersOfAccount(GuidRequest request)
    {
        try
        {
            var smartUsers = await _ctx.DbContext.SmartUsers
            .Where(su => su.AccountId == request.Id)
            .ToListAsync();
            if (smartUsers is null)
            {
                return SmartUserResponse.Failed("Not Found");
            }
            else
            {
                return new SmartUserResponse(smartUsers!);
            }
        }
        catch (Exception ex)
        {
            return SmartUserResponse.Error(ex);
        }
    }

    public async Task<SuccessResponse> Update(UpdateRequest request)
    {
        var SmartUser = await _ctx.DbContext.SmartUsers.FindAsync(request.id);
        if (SmartUser is null)
        {
            return SuccessResponse.Failed("Not Found");
        }
        SmartUser.SmartHomeId = request.SmartHomeId;
        SmartUser.AccountId = request.AccountId;
        SmartUser.Role = request.RoleId;
        try
        {
            await _ctx.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.FailedJson(ex);
        }
    }
}
