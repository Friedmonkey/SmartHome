using SmartHome.Backend.Test.Testing;
using SmartHome.Common.Api;
using SmartHome.Common.Models;
using SmartHome.Database.ApiContext;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IAccountService;
using static SmartHome.Common.Api.ISmartHomeService;

namespace SmartHome.Api.Test;

[Collection("SmartHomeServiceCollection")]
public class SmartHomeServiceTest
{
    private readonly IAccountService _accountService;
    private readonly ISmartHomeService _smartHomeService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public SmartHomeServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _accountService = fixture.TestAccountService;
        _smartHomeService = fixture.TestSmartHomeService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("James", "name", "password1", true)]
    [InlineData("hello@gmail", "name", "Password@01Long", true)]
    public async Task CreateSmartHomeTest(string name, string wifiname, string password, bool expected)
    {
        var resultLogin = await _accountService.Login(_fixture.LoginRequest);
        if (_fixture.WasSuccess(resultLogin))
            _fixture.ApiLogin(resultLogin.JWT);
        else
            TestConsole.WriteLine(resultLogin._RequestMessage);

        var request = new CreateSmartHomeRequest(name, wifiname, password);
        var result = await _smartHomeService.CreateSmartHome(request);
        if (!_fixture.WasSuccess(result))
            TestConsole.WriteLine(result._RequestMessage);

        Assert.Equal(expected, result._RequestSuccess);
    }
}
