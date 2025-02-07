namespace SmartHome.Common.Models;

public record Response<T>(bool Success = true) where T : Response<T>
{
    public static T Failed => (T)Activator.CreateInstance(typeof(T), [false])!;
}

public static class ResponseExtentions
{
    public static bool WasSuccess<T>(this Response<T>? response) where T : Response<T>
    {   //handles null too
        return response?.Success == true;
    }
}