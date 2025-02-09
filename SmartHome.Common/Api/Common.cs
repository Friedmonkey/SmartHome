using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record EmptyRequest();
public record SuccessResponse() : Response<SuccessResponse>
{
    public static SuccessResponse Success() => new SuccessResponse();
};