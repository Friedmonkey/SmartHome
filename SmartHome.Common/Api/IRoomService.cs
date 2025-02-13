using SmartHome.Common.Models;

namespace SmartHome.Common.Api;


public interface IRoomService
{
    public record Response(object Room) : Response<Response>;
    public record RoomsResponse(List<object> Rooms) : Response<RoomsResponse>;
    
    public record CreateRequest(string Name, Guid SmartHomeId);

    public Task<SuccessResponse> Create(CreateRequest request);

    public Task<RoomsResponse> GetRoomsOfSmartHome(GuidRequest request); // return list of Rooms
    
    public Task<SuccessResponse> Delete(GuidRequest request);
    
    public Task<SuccessResponse> Update(CreateRequest request);


}
