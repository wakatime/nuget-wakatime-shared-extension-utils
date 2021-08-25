using System.Text;

namespace WakaTime.Shared.ExtensionUtils
{
    public class ConfigFile
    {
        public readonly string ConfigFilepath;

        public ConfigFile(string configFilepath)
        {
            ConfigFilepath = configFilepath;
        }

        public string GetSetting(string key, string section = "settings")
        {
            var ret = new StringBuilder(255);

            return NativeMethods.GetPrivateProfileString(section, key, "", ret, 255, ConfigFilepath) > 0
                ? ret.ToString()
                : string.Empty;
        }

        public bool GetSettingAsBoolean(string key, bool @default = false, string section = "settings")
        {
            var ret = new StringBuilder(255);

            if (NativeMethods.GetPrivateProfileString(section, key, @default.ToString(), ret, 255, ConfigFilepath) > 0)
            {
                if (bool.TryParse(ret.ToString(), out var parsed))
                    return parsed;
            }

            return @default;
        }

        public void SaveSetting(string section, string key, string value)
        {
            if (bool.TryParse(value.Trim(), out _))
            {
                NativeMethods.WritePrivateProfileString(section, key, value.Trim().ToLower(), ConfigFilepath);
                return;
            }


            NativeMethods.WritePrivateProfileString(section, key, value.Trim(), ConfigFilepath);
        }
    }
}
