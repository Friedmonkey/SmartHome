using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record SmartHomeRequest
{
    public Guid smartHome { get; private set; } = Guid.Empty;
    public void UpdateSmartHome(Guid newGuid)
    { 
        this.smartHome = newGuid;
    }

}; //clearly indicate its a smarthome
public record GuidRequest(Guid Id);
public record GuidResponse(Guid Id) : Response<GuidResponse>;

public record EmptyRequest(string? nothing = null);
public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};