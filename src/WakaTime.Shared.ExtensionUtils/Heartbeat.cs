namespace WakaTime.Shared.ExtensionUtils
{
    public class Heartbeat
    {
        public string Entity { get; set; }
        public string Timestamp { get; set; }
        public string Project { get; set; }
        public bool IsWrite { get; set; }
        public HeartbeatCategory Category { get; set; }
        public EntityType EntityType { get; set; }
    }
}