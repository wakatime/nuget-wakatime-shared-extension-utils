namespace WakaTime.Shared.ExtensionUtils
{
    public class Configuration
    {
        public string EditorVersion { get; set; }
        public string EditorName { get; set; }
        public string PluginName { get; set; }
        public int TimerInterval { get; set; } = 8000;
    }
}