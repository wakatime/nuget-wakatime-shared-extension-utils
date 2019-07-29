using System;
using System.Net;
using System.Text.RegularExpressions;

namespace WakaTime.Shared.ExtensionUtils
{
    internal static class Constants
    {
        internal static string PluginVersion =
            $"{WakaTime.CoreAssembly.Version.Major}.{WakaTime.CoreAssembly.Version.Minor}.{WakaTime.CoreAssembly.Version.Build}";
       
        internal const string CliUrl = "https://github.com/wakatime/wakatime/archive/master.zip";
        internal const string CliFolder = @"wakatime-master\wakatime\cli.py";
    }
}