namespace SmartHome.Backend.Logging
{
    public class Logger
    {
        public void Trace(TraceItem item, params object[] args) 
        {
            string message = string.Format(item.message, args);
            Console.ForegroundColor = item.type switch
            {
                TraceType.Info => ConsoleColor.Cyan,
                TraceType.Warning => ConsoleColor.Yellow,
                TraceType.Error => ConsoleColor.Red,
                _ => ConsoleColor.White
            };
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
