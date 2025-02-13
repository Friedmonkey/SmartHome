﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using SmartHome.Database;
using static SmartHome.Common.Api.IPersonTestingService;

namespace SmartHome.Backend.Api;

public class PersonTestingService : IPersonTestingService
{
    private static List<Person> _persons = new();

    private readonly ApiContext _context;

    public PersonTestingService(ApiContext context)
    {
        _context = context;
    }

    public async Task<DeviceListResponse> TestDb(TestDbRequest request)
    {
        var result = await _context.DbContext.Devices.ToListAsync();
        if (result == null)
            return DeviceListResponse.Failed("Not Devices found in DataBase");
        else
            return new DeviceListResponse(result);
    }

    public async Task<SuccessResponse> AddPerson(AddPersonRequest request)
    {
        if (request.person is null)
            return SuccessResponse.Failed("request person object was null");
        await Task.Delay(500);
        _persons.Add(request.person);
        return SuccessResponse.Success();
    }

    public async Task<PersonResponse> GetPersonByAge(GetPersonByAgeRequest request)
    {
        await Task.Delay(500);
        var person = _persons.FirstOrDefault(p => p.Age == request.age);
        if (person is null)
            return PersonResponse.Failed("person not found");

        return new PersonResponse(person);
    }

    public async Task<PersonResponse> GetPersonByName(GetPersonByNameRequest request)
    {
        await Task.Delay(500);
        var person = _persons.FirstOrDefault(p => p.Name == request.name);
        if (person is null)
            return PersonResponse.Failed($"person with name {request.name} not found");

        return new PersonResponse(person);
    }

   
}
