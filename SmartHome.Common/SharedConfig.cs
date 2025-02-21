namespace SmartHome.Common;

public class SharedConfig
{
    private const string ApiBase = "api/";
    public static class Urls
    {
        private const string AccountBase = ApiBase + "account/";
        public static class Account
        {
            public const string RegisterUrl = AccountBase + "register";
            public const string LoginUrl = AccountBase + "login";
            public const string RefreshUrl = AccountBase + "refresh";
            public const string LogoutUrl = AccountBase + "logout";
            public const string ForgotPasswordUrl = AccountBase + "forgotPassword";
        }

        private const string DeviceBase = ApiBase + "device/";
        public static class Device
        {
            public const string GetAllDevices = DeviceBase + "GetAllDevices";
            public const string UpdateDevicesRange = DeviceBase + "UpdateDevicesRange";
            public const string UpdateDevice = DeviceBase + "UpdateDevice";
            public const string DeleteDevice = DeviceBase + "DeleteDevice";
            public const string CreaateDevice = DeviceBase + "CreateDevice";
            public const string GetAllRooms = DeviceBase + "GetAllRooms";
            public const string UpdateDeviceConfig = DeviceBase + "UpdateDeviceConfig";
        }
        
        private const string RoutineBase = ApiBase + "Routine/";
        public static class Routine
        {
            public const string GetAllRoutines =  RoutineBase + "GetRoutinesOfSmartHomeWithAccess";
            public const string UpdateRoutine = RoutineBase + "UpdateRoutine";
            public const string DeleteRoutine = RoutineBase + "DeleteRoutine";
            public const string CreateRoutine = RoutineBase + "CreateRoutine";
            public const string CreateDeviceAction = RoutineBase + "CreateDeviceAction";
            public const string UpdateDeviceAction = RoutineBase + "UpdateDeviceAction";
            public const string DeleteDeviceAction = RoutineBase + "DeleteDeviceAction";
        }

        private const string RoomBase = ApiBase + "room/";
        public static class Room
        {
            public const string GetAllRooms = RoomBase + "GetAllRooms";
            public const string UpdateRoomName = RoomBase + "UpdateRoomName";
            public const string CreateRoom = RoomBase + "CreateRoom";
            public const string DeleteRoom = RoomBase + "DeleteRoom";
            public const string DeleteRoom2 = RoomBase + "DeleteRoom2";
        }

        private const string LogBase = ApiBase + "log/";
        public static class Log
        {
            public const string GetAllLogs = LogBase + "GetAllLogs";
            public const string CreateLog = RoomBase + "CreateLog";
        }

        private const string SmartHomeBase = ApiBase + "SmartHome/";
        public static class SmartHome
        {
            public const string CreateSmartHomeUrl = SmartHomeBase + "create";
            public const string InviteToSmartHomeUrl = SmartHomeBase + "invite";
            public const string AcceptInviteToSmartHomeUrl = SmartHomeBase + "acceptInvite";
            public const string GetJoinedUrl = SmartHomeBase + "getJoined";
            public const string GetInvitesUrl = SmartHomeBase + "getInvited";
            public const string GetByIDUrl = SmartHomeBase + "getByIDUrl";
            public const string GetAllUsers = SmartHomeBase + "getAllUsers";
        }

        private const string SmartUserBase = ApiBase + "SmartUser/";
        public static class SmartUser
        {
            public const string AddSmartUserUrl = SmartUserBase + "add";
            public const string UpdateSmartHomeUrl = SmartUserBase + "update";
            public const string GetSmartUsersOfSmartUserUrl = SmartUserBase + "getsmartuserofaccount";
            public const string DeleteSmartUserUrl = SmartUserBase + "Delete";
        }
    }
}
