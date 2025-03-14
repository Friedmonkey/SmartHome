using SmartHome.Backend.Api;
using SmartHome.Common.Api;
using SmartHome.Common.Models.Entities;
using SmartHome.Common.Models.Enums;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IDeviceService;
using static SmartHome.Common.Api.IAccountService;
using SmartHome.Common.Models.Configs;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SmartHome.Backend.Test;
[Collection("SmartHomeServiceCollection")]
public class DeviceServiceTest
{
    private readonly IDeviceService _deviceService;
    private readonly IAccountService _accountService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public DeviceServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _deviceService = fixture.TestDeviceService;
        _accountService = fixture.TestAccountService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("Device 1", true)]
    [InlineData("Device 23", true)]
    public async Task CreateDeviceTest(string name, bool expected)
    {
        var resultLogin = await _accountService.Login(_fixture.LoginRequest);
        if (_fixture.WasSuccess(resultLogin))
            _fixture.ApiLogin(resultLogin.JWT);
        else
            TestConsole.WriteLine(resultLogin._RequestMessage);

        var tmp = new Device() 
        { 
            Name = name,
            Type = DeviceType.Lamp,
            Config = new LampConfig()
            {
                Brightness = 100,
                Color = "#FFFFFF",
                Enabled = true,
            },
            RoomId = _fixture.RoomId
        };
        tmp.SaveDeviceConfig();
        var request = new DeviceRequest(tmp);
        var req = request with { smartHome = _fixture.SmartHomeId };
        var result = await _deviceService.CreateDevice(req);
        if (result.Id == Guid.Empty)
        {
            TestConsole.WriteLine(result._RequestMessage);
        }
        Assert.Equal(expected, result.Id != Guid.Empty);
    }

    [Theory]
    [InlineData("Name 123", true)]
    [InlineData("Name 321", true)]
    public async Task UpdateDeviceTest(string name, bool expected)
    {
        var resultLogin = await _accountService.Login(_fixture.LoginRequest);
        if (_fixture.WasSuccess(resultLogin))
            _fixture.ApiLogin(resultLogin.JWT);
        else
            TestConsole.WriteLine(resultLogin._RequestMessage);

        var tmp = new Device()
        {
            Id = _fixture.DeviceId,
            Name = name,
            Type = DeviceType.Lamp,
            Config = new LampConfig()
            {
                Brightness = 100,
                Color = "#FFFFFF",
                Enabled = true,
            },
            RoomId = _fixture.RoomId
        };
        tmp.SaveDeviceConfig();
        var request = new DeviceRequest(tmp);
        var req = request with { smartHome = _fixture.SmartHomeId };
        var result = await _deviceService.UpdateDevice(req);
        if (_fixture.WasSuccess(result))
        {
            TestConsole.WriteLine(result._RequestMessage);
        }
        Assert.Equal(expected, _fixture.WasSuccess(result));
    }
}
