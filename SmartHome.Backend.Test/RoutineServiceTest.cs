using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IRoutineService;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Backend.Test;
[Collection("SmartHomeServiceCollection")]
public class RoutineServiceTest
{
    private readonly IRoutineService _routineService;
    private readonly IAccountService _accountService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public RoutineServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _routineService = fixture.TestRoutineService;
        _accountService = fixture.TestAccountService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("Name",(byte)(RoutineRepeat.Monday | RoutineRepeat.Saturday), true)]
    [InlineData("Name 1",(byte)(RoutineRepeat.Monday | RoutineRepeat.Saturday), true)]
    public async Task CreateRoutineTest(string name, byte repeatDays, bool expected)
    {
        var resultLogin = await _accountService.Login(_fixture.LoginRequest);
        if (_fixture.WasSuccess(resultLogin))
            _fixture.ApiLogin(resultLogin.JWT);
        else
            TestConsole.WriteLine(resultLogin._RequestMessage);

        var tmp = new Routine() 
        { 
            Name = name,
            RepeatDays = repeatDays,
            Start = TimeOnly.MinValue,
            SmartHomeId = _fixture.SmartHomeId,
        };
        var request = new RoutineRequest(tmp);
        var req = request with { smartHome = _fixture.SmartHomeId };
        var result = await _routineService.CreateRoutine(req);
        if (result.Id == Guid.Empty)
        {
            TestConsole.WriteLine(result._RequestMessage);
        }
        Assert.Equal(expected, result.Id != Guid.Empty);
    }
}
