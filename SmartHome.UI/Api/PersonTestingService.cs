using SmartHome.Common;
using SmartHome.Common.Api;
using static SmartHome.Common.Api.IPersonTestingService;

namespace SmartHome.UI.Api;

public class PersonTestingService : IPersonTestingService
{
    private readonly ApiService _api;

    public PersonTestingService(ApiService api)
    {
        this._api = api;
    }

    public async Task<SuccessResponse> AddPerson(AddPersonRequest request)
    {
        return await _api.Post<SuccessResponse>(SharedConfig.Urls.Person.AddPersonUrl, request);
    }

    public async Task<PersonResponse> GetPersonByAge(GetPersonByAgeRequest request)
    {
        return await _api.Get<PersonResponse>(SharedConfig.Urls.Person.GetByAgeUrl, request);
    }

    public async Task<PersonResponse> GetPersonByName(GetPersonByNameRequest request)
    {
        return await _api.Get<PersonResponse>(SharedConfig.Urls.Person.GetByNameUrl, request);
    }
}
