using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IRoutineService;

namespace SmartHome.Backend.Test;
[Collection("SmartHomeServiceCollection")]
public class RoutineServiceTest
{
    private readonly IRoutineService _routineService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public RoutineServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _routineService = fixture.TestRoutineService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("Name",(byte)(RoutineRepeat.Monday | RoutineRepeat.Saturday), true, false)]
    [InlineData("Name",(byte)(RoutineRepeat.Monday | RoutineRepeat.Saturday), false, true)]
    public async Task CreateRoutine(string name, byte repeatDays, bool newGuid, bool expected)
    {
        var tmp = new Routine() 
        { 
            Name = name,
            RepeatDays = repeatDays,
            Start = TimeOnly.MinValue,
        };
        if (newGuid)
            tmp.SmartHomeId = Guid.NewGuid();
        else
            tmp.SmartHomeId = _fixture.SmartHomeId;
        
        var request = new RoutineRequest(tmp);
        var result = await _routineService.CreateRoutine(request);
        if (result.Id == Guid.Empty)
        {
            TestConsole.WriteLine(result._RequestMessage);
        }
        Assert.Equal(expected, result.Id != Guid.Empty);
    }
}
