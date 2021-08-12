using System;
using System.IO;
using System.Text;

namespace WakaTime.Shared.ExtensionUtils
{
    public class ConfigFile
    {
        public string ApiKey { get; set; }
        public string Proxy { get; set; }
        public bool Debug { get; set; }
        public bool StatusBarEnabled { get; set; }
        public bool StatusBarCodingActivity { get; set; }

        private readonly string _configFilepath;

        public ConfigFile()
        {
            _configFilepath = GetConfigFilePath();

            Read();
        }

        public void Read()
        {
            var ret = new StringBuilder(2083);

            ApiKey = NativeMethods.GetPrivateProfileString("settings", "api_key", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            Proxy = NativeMethods.GetPrivateProfileString("settings", "proxy", "", ret, 2083, _configFilepath) > 0
                ? ret.ToString()
                : string.Empty;

            if (NativeMethods.GetPrivateProfileString("settings", "debug", "", ret, 2083, _configFilepath) > 0 &&
                bool.TryParse(ret.ToString(), out var debug))
                Debug = debug;

            if (NativeMethods.GetPrivateProfileString("settings", "status_bar_enabled", "", ret, 2083, _configFilepath) > 0 &&
                bool.TryParse(ret.ToString(), out var statusBarEnabled))
                StatusBarEnabled = statusBarEnabled;

            // ReSharper disable once InvertIf
            if (NativeMethods.GetPrivateProfileString("settings", "status_bar_coding_activity", "", ret, 2083, _configFilepath) > 0 &&
            bool.TryParse(ret.ToString(), out var statusBarCodingActivityEnabled))
                StatusBarCodingActivity = statusBarCodingActivityEnabled;
        }

        // ReSharper disable once UnusedMember.Global
        public void Save()
        {
            if (!string.IsNullOrEmpty(ApiKey))
                NativeMethods.WritePrivateProfileString("settings", "api_key", ApiKey.Trim(), _configFilepath);

            NativeMethods.WritePrivateProfileString("settings", "proxy", Proxy.Trim(), _configFilepath);
            NativeMethods.WritePrivateProfileString("settings", "debug", Debug.ToString().ToLower(), _configFilepath);
            NativeMethods.WritePrivateProfileString("settings", "status_bar_enabled", StatusBarEnabled.ToString().ToLower(), _configFilepath);
            NativeMethods.WritePrivateProfileString("settings", "status_bar_coding_activity", StatusBarCodingActivity.ToString().ToLower(), _configFilepath);
        }

        private static string GetConfigFilePath()
        {
            var homeFolder = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            return Path.Combine(homeFolder, ".wakatime.cfg");
        }
    }
}
