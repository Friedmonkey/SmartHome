using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record EmptyRequest(string? nothing = null);
public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};