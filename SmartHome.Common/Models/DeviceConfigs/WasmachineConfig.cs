using System;

namespace SmartHome.Common.Models.Configs;

public class WasmachineConfig
{
    public bool Ingeschakeld { get; set; }
    public string Programma { get; set; }
    public DateTime ProgrammaStart { get; set; }
    public int ProgrammaDuur { get; set; }
}
