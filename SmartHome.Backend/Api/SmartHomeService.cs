using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.Backend.Api;

public class SmartHomeService : ISmartHomeService
{
    private readonly ApiContext _apiContext;

    public SmartHomeService(ApiContext apiContext)
    {
        _apiContext = apiContext;
    }
    public async Task<SuccessResponse> CreateSmartHome(CreateSmartHomeRequest request)
    {
       Home home = new(){ Id = Guid.NewGuid(), Name = request.name, SSID = request.ssId, SSPassword = request.ssPassword};
       try
       {
           var result = await _apiContext.DbContext.Home.AddAsync(home);
           await _apiContext.DbContext.SaveChangesAsync();
           if (result is null)
               return SuccessResponse.Failed("SmartHomeRequest was null");
           else
           {
               return SuccessResponse.Success();
           }
       }
       catch (Exception ex) {
           return SuccessResponse.FailedJson(ex);
       }
    }

    public async Task<SuccessResponse> DeleteSmartHome(RequestByGuid request)
    {
        try
        {
            var deletedhome = await (_apiContext.DbContext.Home.Where(h => h.Id == request.Id)).FirstOrDefaultAsync();
            if (deletedhome is null)
            {
                return SuccessResponse.Failed("Not Found");
            }
            var result = _apiContext.DbContext.Home.Remove(deletedhome);
            if (result is null)
                return SuccessResponse.Failed("SmartHomeRequest was null");
            else
            {
                await _apiContext.DbContext.SaveChangesAsync();
                return SuccessResponse.Success();
            }
        }
        catch (Exception ex)
        {
            return SuccessResponse.FailedJson(ex);
        }
    }

    public async Task<SmartHomeResponse> GetSmartHomesOfSmartUser(RequestByGuid request)
    {
        List<Home?> homes =  await _apiContext.DbContext.SmartUser
            .Where(su => su.AccountId == request.Id)
            .Select(su => su.SmartHome)
            .ToListAsync();
        if (homes is null)
        {
            return SmartHomeResponse.Failed("Not Found");
        }
        else
        {
            return new SmartHomeResponse(homes!);
        }
    }

    public async Task<SuccessResponse> UpdateSmartHome(UpdateSmartHomeRequest request)
    {
        var home = await _apiContext.DbContext.Home.FindAsync(request.Id);
        if (home is null)
            return SuccessResponse.Failed("Not Found");

        home.Name = request.name;
        home.SSID = request.ssId;
        home.SSPassword = request.ssPassword;

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
