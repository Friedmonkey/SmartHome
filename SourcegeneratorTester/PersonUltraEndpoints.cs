﻿using SmartHome.Common.Models;
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
    private readonly IDatabase _db;

    public PersonUltraEndpoints(IDatabase db)
    {
        this._db = db;
    }

    [UltraGet("api/persons/getbyname_testing")]
    public async Task<Person> GetPersonWithName(string nam)
    {
        return (await _db.GetPersonByName(nam)) ?? throw new Exception("person not found");
    }
}
