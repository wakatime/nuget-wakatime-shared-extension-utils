namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class FlagHolderExtension
    {
        public static void Send(this FlagHolder heartbeat)
        {
            heartbeat.WakaTime.HandleActivity(heartbeat);
        }
    }
}