using SmartHome.Common.Models;

namespace SmartHome.Common.Api;

public record EmptyRequest();
public record EmptyResponse() : Response<EmptyResponse>
{
    public static EmptyResponse Success() => new EmptyResponse();
};

public record TokenResponse(string JWT, string Refresh) : Response<TokenResponse>;
