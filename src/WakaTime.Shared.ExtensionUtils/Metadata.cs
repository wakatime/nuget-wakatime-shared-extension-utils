namespace WakaTime.Shared.ExtensionUtils
{
    /// <summary>
    ///     Metadata for the plugin
    /// </summary>
    public class Metadata
    {
        /// <summary>
        ///     The version of the editor
        /// </summary>
        public string EditorVersion { get; set; }
        
        /// <summary>
        ///     The name of the editor (e.g. Visual Studio, IntelliJ IDEA, MyIDE, etc.)
        /// </summary>
        public string EditorName { get; set; }
        
        /// <summary>
        ///     The version of the plugin
        /// </summary>
        public string PluginVersion { get; set; }
        
        /// <summary>
        ///     The name of the plugin (e.g. MyWakaTime, MyPlugin, etc.)
        /// </summary>
        public string PluginName { get; set; }
    }
}