using SmartHome.Common.Models;
using UltraEndpoints.Generator.Attributes;

namespace SmartHome.Backend.UltraEndpoints;

[UltraEndpoint]
public class balls
{ 

}

//[UltraEndpoint]
//public class PersonUltraEndpoints
//{
//    private readonly IDatabase _db;

//    public PersonUltraEndpoints(IDatabase db)
//    {
//        this._db = db;
//    }

//    [UltraGet("api/persons/getbynamebackend")]
//    public async Task<Person> GetPersonWithName(string nam)
//    {
//        return (await _db.GetPersonByName(nam)) ?? throw new Exception("person not found");
//    }
//}
