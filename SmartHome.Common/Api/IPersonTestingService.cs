using SmartHome.Common.Models;
using SmartHome.Common.Models.Entities;

namespace SmartHome.Common.Api;

public record PersonResponse(Person person) : Response<PersonResponse>;

public record DeviceListResponse(List<Device> device) : Response<DeviceListResponse>;
public interface IPersonTestingService
{
    public record TestDbRequest(string HomeGuid);
    Task<DeviceListResponse> TestDb(TestDbRequest request);

    public record AddPersonRequest(Person person);
    Task<SuccessResponse> AddPerson(AddPersonRequest request);


    public record GetPersonByAgeRequest(int age);
    Task<PersonResponse> GetPersonByAge(GetPersonByAgeRequest request);

    public record GetPersonByNameRequest(string name);
    Task<PersonResponse> GetPersonByName(GetPersonByNameRequest request);
}
