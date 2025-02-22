namespace SmartHome.Common.Models.Configs;

public class WashingmachineConfig : DeviceConfig
{
    public bool Enabled { get; set; }
    public string Program { get; set; }
    public DateTime ProgramStart { get; set; }
    public int ProgrammaDuration { get; set; }
}
