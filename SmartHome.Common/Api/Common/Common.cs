using SmartHome.Common.Models;

namespace SmartHome.Common.Api.Common;


public record SmartHomeGuidRequest(Guid Id) : SmartHomeRequest;
public record GuidRequest(Guid Id);
public record GuidResponse(Guid Id) : Response<GuidResponse>;

public record EmptySmartHomeRequest(string? nothing = null) : SmartHomeRequest;
public record EmptyRequest(string? nothing = null);


public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};

//inherit this when the request is related to smarthome, because this auto injects the currect smarthome guid
public abstract record SmartHomeRequest
{
    public Guid smartHome { get; init; } = Guid.Empty;
    //public void UpdateSmartHome(Guid newGuid)
    //{
    //    this.smartHome = newGuid;
    //}

}; //clearly indicate its a smarthome