namespace SmartHome.Common.Models.Auth;

public record GenericSuccessResponse() : Response<GenericSuccessResponse> 
{
    public static GenericSuccessResponse Success() => new GenericSuccessResponse();
};
