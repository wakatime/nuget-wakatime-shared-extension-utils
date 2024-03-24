namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class CliHeartbeatExtension
    {
        public static void Send(this CliHeartbeat heartbeat)
        {
            heartbeat.WakaTime.HandleActivity(heartbeat);
        }
    }
}