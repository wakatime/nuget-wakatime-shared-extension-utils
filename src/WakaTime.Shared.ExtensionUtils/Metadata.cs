namespace WakaTime.Shared.ExtensionUtils
{
    public class Metadata
    {
        public string EditorVersion { get; set; }
        public string EditorName { get; set; }
        public string PluginVersion { get; set; }
        public string PluginName { get; set; }

        /// <summary>
        ///     The language of the project.
        /// </summary>
        /// <remarks>
        ///     If set allows to pass the '--language' parameter to the WakaTime CLI.
        /// </remarks>
        public string Language { get; set; }
    }
}