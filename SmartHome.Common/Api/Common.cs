using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record SmartHomeRequest(Guid smartHome); //clearly indicate its a smarthome
public record GuidRequest(Guid Id);
public record GuidResponse(Guid Id) : Response<GuidResponse>;

public record EmptyRequest(string? nothing = null);
public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};