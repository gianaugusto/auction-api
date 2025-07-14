using System;

namespace AutoAuction.CrossCutting.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void LogError(string message, Exception exception);
    }
}
