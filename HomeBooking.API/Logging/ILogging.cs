namespace HomeBooking.API.Logging
{
    public interface ILogging
    {
        void Log(LoggingStatusEnum type, string message);
    }
}
