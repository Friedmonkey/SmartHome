using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IRoomService;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Backend.Test;
[Collection("SmartHomeServiceCollection")]
public class RoomServiceTest
{
    private readonly IRoomService _roomService;
    private readonly IAccountService _accountService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public RoomServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _roomService = fixture.TestRoomService;
        _accountService = fixture.TestAccountService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("Name", true)]
    [InlineData("Name 1", true)]
    public async Task CreateRoutine(string name, bool expected)
    {
        var resultLogin = await _accountService.Login(_fixture.LoginRequest);
        if (_fixture.WasSuccess(resultLogin))
            _fixture.ApiLogin(resultLogin.JWT);
        else
            TestConsole.WriteLine(resultLogin._RequestMessage);

        var tmp = new Room() 
        { 
            Name = name,
            SmartHomeId = _fixture.SmartHomeId,
        };
        var request = new RoomRequest(tmp);
        var req = request with { smartHome = _fixture.SmartHomeId };
        var result = await _roomService.CreateRoom(req);
        if (result.Id == Guid.Empty)
        {
            TestConsole.WriteLine(result._RequestMessage);
        }
        Assert.Equal(expected, result.Id != Guid.Empty);
    }
}
