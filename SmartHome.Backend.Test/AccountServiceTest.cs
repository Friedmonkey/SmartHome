using SmartHome.Common.Api;
using SmartHome.Common.Models;
using Xunit.Abstractions;
using static SmartHome.Common.Api.IAccountService;

namespace SmartHome.Api.Test;

[Collection("SmartHomeCollection")]
public class AccountServiceTest
{
    private readonly IAccountService _accountService;
    private ITestOutputHelper TestConsole { get; }

    public AccountServiceTest(SmartHomeServiceFixtureSetupLogic fixture, ITestOutputHelper testConsole)
    {
        _accountService = fixture.TestAccountService;
        TestConsole = testConsole;
    }

    [Theory]
    [InlineData("hello@mail", "name", "password1", "password2", false)]
    [InlineData("hello@mail", "name", "Password@01Long", "Password@01Long", true)]
    public async Task CreateAccountTest(string Email, string Username, string Password, string PasswordConfirm, bool expected)
    {
        var request = new RegisterRequest(Email, Username, Password, PasswordConfirm);
        var result = await _accountService.Register(request);
        if (!WasSuccess(result))
            TestConsole.WriteLine(result._RequestMessage);

        Assert.Equal(expected, result._RequestSuccess);
    }

    [Theory]
    [InlineData("hello@mail", "name", "password1", "password2", false)]
    [InlineData("hello@mail", "name", "Password@01Long", "Password@01Long", true)]
    public async Task LoginTest(string Email, string Username, string Password, string PasswordConfirm, bool expected)
    {
        var request = new RegisterRequest(Email, Username, Password, PasswordConfirm);
        var result = await _accountService.Login(request);
        if (!WasSuccess(result))
            TestConsole.WriteLine(result._RequestMessage);

        Assert.Equal(expected, result._RequestSuccess);
    }

    public static bool WasSuccess<T>(Response<T>? response) where T : Response<T>
    {   //handles null too
        return response?._RequestSuccess == true;
    }
}
