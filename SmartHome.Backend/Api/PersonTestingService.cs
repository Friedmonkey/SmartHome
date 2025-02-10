using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using static SmartHome.Common.Api.IPersonTestingService;

namespace SmartHome.Backend.Api;

public class PersonTestingService : IPersonTestingService
{
    private static List<Person> _persons = new();

    public async Task<SuccessResponse> AddPerson(AddPersonRequest request)
    {
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
