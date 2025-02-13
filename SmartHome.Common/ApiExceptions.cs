namespace SmartHome.Common
{
    public class ApiError : Exception
    {
        public ApiError(string error, bool fatal = false) : base(error)
        {
            Error = error;
            IsFatal = fatal;
        }

        public bool IsFatal { get; init; }
        public string Error { get; init; }
    }
}
