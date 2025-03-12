using SmartHome.Common.Api;
using SmartHome.Common.Models;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Api.Test;

[Collection("SmartHomeServiceCollection")]
public class AccountServiceTest
{
    private readonly IAccountService _accountService;
    private readonly SmartHomeServiceFixtureSetupLogic _fixture;
    private ITestOutputHelper TestConsole { get; }

    public AccountServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _fixture = fixture;
        _accountService = fixture.TestAccountService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("hello@gmail", "name", "password1", "password2", false)]
    [InlineData("hello@gmail.com", "name", "Password@01Long", "Password@01Long", true)]
    public async Task CreateAccountTest(string Email, string Username, string Password, string PasswordConfirm, bool expected)
    {
        var request = new RegisterRequest(Email, Username, Password, PasswordConfirm);
        var result = await _accountService.Register(request);
        if (!WasSuccess(result))
            TestConsole.WriteLine(result._RequestMessage);

        Assert.Equal(expected, result._RequestSuccess);
    }

    [Theory]
    [InlineData("Admin@gmail.com", "Password1!", true)]
    [InlineData("hello@gmail.com", "password1", false)]
    [InlineData("hello@gmail.com", "Password@01Long@01Long", false)]
    public async Task LoginTest(string Email, string Password, bool expected)
    {
        var request = new LoginRequest(Email, Password);
        var result = await _accountService.Login(request);
        if (WasSuccess(result))
            _fixture.ApiLogin(result.JWT);
        else
            TestConsole.WriteLine(result._RequestMessage);
       
        Assert.Equal(expected, result._RequestSuccess);
    }

    public static bool WasSuccess<T>(Response<T>? response) where T : Response<T>
    {   //handles null too
        return response?._RequestSuccess == true;
    }
}
