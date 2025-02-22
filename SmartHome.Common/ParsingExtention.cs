namespace SmartHome.Common;

public static class ParsingExtention
{
    public static Guid? GuidTryParse(string? guidStr)
    {
        if (guidStr == null)
            return null;
        var success = Guid.TryParse(guidStr, out Guid guid);
        if (success) return guid;
        return null;
    }
}
