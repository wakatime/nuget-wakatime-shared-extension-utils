namespace WakaTime.Shared.ExtensionUtils
{
    public class Heartbeat
    {
        public string Entity { get; set; }
        public string Timestamp { get; set; }
        public string Project { get; set; }
        public bool IsWrite { get; set; }
        public HeartbeatCategory? Category { get; set; }
        public EntityType? EntityType { get; set; }

        /// <summary>
        /// It's a workaround for serialization.
        /// More details https://bit.ly/3mJB1mP
        /// </summary>
        public override string ToString()
        {
            return $"{{\"entity\":\"{Entity.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"," +
                $"\"timestamp\":{Timestamp}," +
                $"\"project\":\"{Project.Replace("\"", "\\\"")}\"," +
                $"\"is_write\":{IsWrite.ToString().ToLower()}," +
                $"\"category\":\"{Category.GetDescription()}\"," +
                $"\"entity_type\":\"{EntityType.GetDescription()}\"}}";
        }
    }
}
