namespace WakaTime.Shared.ExtensionUtils
{
    internal static class Constants
    {
        internal static string PluginVersion =
            $"{WakaTime.CoreAssembly.Version.Major}.{WakaTime.CoreAssembly.Version.Minor}.{WakaTime.CoreAssembly.Version.Build}";
       
        internal const string CliUrl = "https://github.com/wakatime/wakatime/archive/master.zip";
        internal const string CliFolder = @"wakatime-master\wakatime\cli.py";
        internal const string StandaloneCli = "wakatime";
        internal const string S3UrlPrefix = "https://wakatime-cli.s3-us-west-2.amazonaws.com/";
    }
}