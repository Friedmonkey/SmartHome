namespace SmartHome.Backend.Logging
{
    public enum TraceType
    { 
        Info,
        Error,
        Warning,
    }
    public record TraceItem(TraceType type, string message);
}
