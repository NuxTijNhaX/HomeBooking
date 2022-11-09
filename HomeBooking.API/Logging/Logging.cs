namespace HomeBooking.API.Logging
{
    public class Logging : ILogging
    {
        public void Log(LoggingStatusEnum type, string message)
        {
            ConsoleColor[] color = { ConsoleColor.Black, ConsoleColor.Green, ConsoleColor.Yellow, ConsoleColor.Red };

            switch (type)
            {
                case LoggingStatusEnum.Error:
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Error];
                    Console.WriteLine($"{LoggingStatusEnum.Error}:::{message}");
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Default];
                    break;
                case LoggingStatusEnum.Warning:
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Warning];
                    Console.WriteLine($"{LoggingStatusEnum.Warning}:::{message}");
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Default];
                    break;
                case LoggingStatusEnum.Information:
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Information];
                    Console.WriteLine($"{LoggingStatusEnum.Information}:::{message}");
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Default];
                    break;
                default:
                    Console.ForegroundColor = color[(int)LoggingStatusEnum.Default];
                    Console.WriteLine($"Undifined:::{message}");
                    break;
            }
        }
    }
}
