using SmartHome.Common.Models;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.Common.Api;


public interface IRoutineService
{
    public record Response(object Routine) : Response<Response>;
    public record RoutinesResponse(List<object> Routines) : Response<RoutinesResponse>;
    
    public record CreateRequest(string Name, Guid SmartHomeId, DateTime Start, byte RepeatDays);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<RoutinesResponse> GetRoutineOfSmartHome(RequestByGuid request); // return list of Rooms
    
    public Task<SuccessResponse> Delete(RequestByGuid request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
