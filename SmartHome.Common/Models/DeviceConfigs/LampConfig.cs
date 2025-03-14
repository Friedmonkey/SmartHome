﻿namespace SmartHome.Common.Models.Configs;

public class LampConfig : DeviceConfig
{
    public bool Enabled { get; set; }

    public int Brightness { get; set; } = 100;

    public string Color { get; set; } = "#FFFFFF";
}
