using Microsoft.EntityFrameworkCore;
using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.ISmartUserService;

namespace SmartUser.Backend.Api;

public class SmartUserService : ISmartUserService
{
    private readonly ApiContext _apiContext;

    public SmartUserService(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }

    public async Task<SuccessResponse> Create(CreateRequest request)
    {
        try
        {
            var result = await _apiContext.DbContext.SmartUser.AddAsync(new()
            {
                Id = Guid.NewGuid(),
                AccountId = request.AccountId,
                RoleId = request.RoleId,
                SmartHomeId = request.SmartHomeId
            });
            await _apiContext.DbContext.SaveChangesAsync();
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
            var result = await _apiContext.DbContext.SmartUser.FindAsync(request.Id);
            if (result == null) {
                return SuccessResponse.Failed("Smart User not found");
            }
            _apiContext.DbContext.SmartUser.Remove(result);
            await _apiContext.DbContext.SaveChangesAsync();
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
            var smartUsers = await _apiContext.DbContext.SmartUser
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
        var SmartUser = await _apiContext.DbContext.SmartUser.FindAsync(request.id);
        if (SmartUser is null)
        {
            return SuccessResponse.Failed("Not Found");
        }
        SmartUser.SmartHomeId = request.SmartHomeId;
        SmartUser.AccountId = request.AccountId;
        SmartUser.RoleId = request.RoleId;
        try
        {
            await _apiContext.DbContext.SaveChangesAsync();
            return SuccessResponse.Success();
        }
        catch (Exception ex)
        {
            return SuccessResponse.FailedJson(ex);
        }
    }
}
