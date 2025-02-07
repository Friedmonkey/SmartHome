namespace SmartHome.Common;

public class SharedConfig
{
    private const string AuthBase = "auth/";
    public static class AuthUrls
    {
        public const string RegisterUrl = AuthBase + "register";
        public const string LoginUrl = AuthBase + "login";
        public const string RefreshUrl = AuthBase + "refresh";
    }

    private const string ApiBase = "api/";
    public static class Urls
    {
        public const string WeatherUrl = ApiBase + "weather";


        private const string AccountBase = ApiBase + "account/";
        public static class Account
        { 
            public const string ForgotPasswordUrl = AccountBase + "forgotPassword";
        }
    }
}
