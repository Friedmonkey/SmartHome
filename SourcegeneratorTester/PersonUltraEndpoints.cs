using SmartHome.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraEndpoints.Generator;

namespace SmartHome.Common.UltraEndpoints;

[UltraEndpoint]
public class PersonUltraEndpoints
{
    [UltraInject]
    public required IDatabase _db { get; set; }

    [UltraGet("api/persons/getbyname")]
    public async Task<Person> GetPersonWithName(string nam)
    {
        return (await _db.GetPersonByName(nam)) ?? throw new Exception("person not found");
    }
}
