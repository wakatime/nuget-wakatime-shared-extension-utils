using System;

namespace WakaTime.Shared.ExtensionUtils
{
    public interface ILogger
    {
        void Debug(string message);
        void Error(string message, Exception ex = null);
        void Warning(string message);
        void Info(string message);
    }
}