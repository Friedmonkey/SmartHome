namespace SmartHome.Common;

public class SharedConfig
{
    private const string ApiBase = "api/";
    public static class Urls
    {
        public const string WeatherUrl = ApiBase + "weather";


        private const string AccountBase = ApiBase + "account/";
        public static class Account
        { 
            public const string RegisterUrl = AccountBase + "register";
            public const string LoginUrl = AccountBase + "login";
            public const string RefreshUrl = AccountBase + "refresh";
            public const string LogoutUrl = AccountBase + "logout";
            public const string ForgotPasswordUrl = AccountBase + "forgotPassword";
        }

        private const string PersonBase = ApiBase + "person/";
        public static class Person
        {
            public const string AddPersonUrl = PersonBase + "add";
            public const string GetDevices = PersonBase + "GetDevices";
            public const string GetByAgeUrl = PersonBase + "getByAge";
            public const string GetByNameUrl = PersonBase + "getByName";
        }

        private const string SmartHomeBase = ApiBase + "SmartHome/";
        public static class SmartHome
        {
            public const string AddPersonUrl = SmartHomeBase + "add";
            public const string GetByAgeUrl = SmartHomeBase + "getByAge";
            public const string GetByNameUrl = SmartHomeBase + "getByName";
        }
    }
}
