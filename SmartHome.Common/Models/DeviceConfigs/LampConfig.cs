namespace SmartHome.Common.Models.Configs;

public class LampConfig : DeviceConfig
{
    public bool Ingeschakeld { get; set; }

    public int Helderheid { get; set; }

    public string Kleur { get; set; }
}
