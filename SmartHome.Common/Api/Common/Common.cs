using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record SmartHomeGuidRequest(Guid Id) : SmartHomeRequest;
public record GuidRequest(Guid Id);
public record GuidResponse(Guid Id) : Response<GuidResponse>;

//Fastendpoints cant handle empty stuff so we put garbage that we dont have to use
public record EmptySmartHomeRequest(string? nothing = null) : SmartHomeRequest;
public record EmptyRequest(string? nothing = null);


public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};

//inherit this when the request is related to smarthome, because this auto injects the current smarthome guid
public abstract record SmartHomeRequest
{
    //No need to set this property, this property gets auto injected from the ApiService
    public Guid smartHome { get; init; } = Guid.Empty;
};