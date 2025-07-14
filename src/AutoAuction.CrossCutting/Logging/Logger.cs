using System;

namespace AutoAuction.CrossCutting.Logging
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"[LOG]: {message}");
        }

        public void LogError(string message, Exception exception)
        {
            Console.WriteLine($"[ERROR]: {message} - {exception.Message}");
        }
    }
}
