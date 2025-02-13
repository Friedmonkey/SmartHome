using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record RequestByGuid(Guid Id);

public interface IRoomService
{
    public record Response(object Room) : Response<Response>;
    public record RoomsResponse(List<object> Rooms) : Response<RoomsResponse>;
    
    public record CreateRequest(string Name, Guid SmartHomeId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<RoomsResponse> GetRoomsOfSmartHome(RequestByGuid request); // return list of Rooms
    
    public Task<SuccessResponse> Delete(RequestByGuid request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
