using SmartHome.Common.Api.Common;
using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record PersonResponse(Person person) : Response<PersonResponse>;

public interface IPersonTestingService
{
    public record AddPersonRequest(Person person);
    Task<SuccessResponse> AddPerson(AddPersonRequest request);


    public record GetPersonByAgeRequest(int age);
    Task<PersonResponse> GetPersonByAge(GetPersonByAgeRequest request);

    public record GetPersonByNameRequest(string name);
    Task<PersonResponse> GetPersonByName(GetPersonByNameRequest request);
}
