using System;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace WakaTime.Shared.ExtensionUtils
{
    internal enum LogLevel
    {
        Debug = 1,
        Info,
        Warning,
        HandledException
    }

    public static class Logger
    {
        private static readonly ConfigFile Config;
        private static IVsOutputWindowPane _wakatimeOutputWindowPane;
        private static IVsOutputWindowPane WakatimeOutputWindowPane =>
            _wakatimeOutputWindowPane ?? (_wakatimeOutputWindowPane = GetWakatimeOutputWindowPane());

        static Logger()
        {
            Config = new ConfigFile();
            Config.Read();
        }

        private static IVsOutputWindowPane GetWakatimeOutputWindowPane()
        {
            if (!(Package.GetGlobalService(typeof(SVsOutputWindow)) is IVsOutputWindow outputWindow)) return null;

            var outputPaneGuid = new Guid(GuidList.GuidWakatimeOutputPane.ToByteArray());

            outputWindow.CreatePane(ref outputPaneGuid, "WakaTime", 1, 1);
            outputWindow.GetPane(ref outputPaneGuid, out var windowPane);

            return windowPane;
        }

        public static void Debug(string message)
        {
            if (!Config.Debug)
                return;

            Log(LogLevel.Debug, message);
        }

        public static void Error(string message, Exception ex = null)
        {
            var exceptionMessage = $"{message}: {ex}";

            Log(LogLevel.HandledException, exceptionMessage);
        }

        public static void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public static void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        private static void Log(LogLevel level, string message)
        {
            var outputWindowPane = WakatimeOutputWindowPane;
            if (outputWindowPane == null) return;

            var outputMessage =
                $"[WakaTime {Enum.GetName(level.GetType(), level)} {DateTime.Now.ToString("hh:mm:ss tt", CultureInfo.InvariantCulture)}] {message}{Environment.NewLine}";

            outputWindowPane.OutputString(outputMessage);
        }
    }
}