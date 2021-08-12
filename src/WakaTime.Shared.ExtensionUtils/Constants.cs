using System.Runtime.InteropServices;

namespace WakaTime.Shared.ExtensionUtils
{
    public static class Constants
    {
        public static readonly string PluginVersion =
            $"{WakaTime.CoreAssembly.Version.Major}.{WakaTime.CoreAssembly.Version.Minor}.{WakaTime.CoreAssembly.Version.Build}";

        internal static readonly string CliFolder =
            $@"wakatime-cli\wakatime-cli{(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : "")}";

        internal const string S3UrlPrefix = "https://wakatime-cli.s3-us-west-2.amazonaws.com/";

        internal const int HeartbeatFrequency = 2; // minutes
    }
}
