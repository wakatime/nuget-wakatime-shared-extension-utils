using WakaTime.Shared.ExtensionUtils.Tests.Stubs;

namespace WakaTime.Shared.ExtensionUtils.Tests
{
    public static class Globals
    {
        public static ILogger LoggerStub = new LoggerStub();

        public static Metadata Metadata = new Metadata()
        {
            EditorName = "xUnit Test",
            EditorVersion = "1.0.0",
            PluginName = "xUnit Test Plugin",
            PluginVersion = "0.0.1"
        };
    }
}