using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Common.Data
{
    public interface IData
    {
        string Str(string key);
        string Int(string key);
        string Bool(string key);
        string Data(string key);
    }
}
