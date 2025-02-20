namespace SmartHome.Common.Models.Enums;

[Flags]
public enum RoutineRepeat : byte
{
    None = 0b0000_0000,
    RepeatWeekly = 0b1000_0000,
    Monday = 0b0100_0000,
    Tuesday = 0b0010_0000,
    Wednsday = 0b0001_0000,
    Thursday = 0b0000_1000,
    Friday = 0b0000_0100,
    Saturday = 0b0000_0010,
    Sunday = 0b0000_0001,
}

static class ConfigHelper
{
    public static RoutineRepeat Add(this RoutineRepeat value, RoutineRepeat flag) => value | flag;
    public static RoutineRepeat Remove(this RoutineRepeat value, RoutineRepeat flag) => value & ~flag;
    public static bool Has(this RoutineRepeat value, RoutineRepeat flag) => value.HasFlag(flag);
}