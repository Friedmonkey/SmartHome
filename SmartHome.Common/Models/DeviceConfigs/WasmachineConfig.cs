using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Models.Configs
{
    public class WasmachineConfig
    {
        public bool Ingeschakeld { get; set; }
        public string Programma { get; set; }
        public DateTime ProgrammaStart { get; set; }
        public int ProgrammaDuur { get; set; }
    }
}
