using System.Text;

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
        public int? LineNumber { get; set; }
        public int? CursorPosition { get; set; }
        public int? Lines { get; set; }
        public string AlternateLanguage { get; set; }
        public bool IsUnsavedEntity { get; set; }

        /// <summary>
        /// Workspace root folder. Passed to wakatime-cli as --project-folder,
        /// which applies to the whole invocation, so only the value from the
        /// first heartbeat in a batch is used.
        /// </summary>
        public string ProjectFolder { get; set; }

        /// <summary>
        /// It's a workaround for serialization.
        /// More details https://bit.ly/3mJB1mP
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{{\"entity\":\"{JsonEscape(Entity)}\",");
            sb.Append($"\"timestamp\":{Timestamp},");
            sb.Append($"\"alternate_project\":\"{JsonEscape(Project)}\",");
            sb.Append($"\"is_write\":{IsWrite.ToString().ToLower()},");

            if (LineNumber != null)
                sb.Append($"\"lineno\":{LineNumber},");

            if (CursorPosition != null)
                sb.Append($"\"cursorpos\":{CursorPosition},");

            if (Lines != null)
                sb.Append($"\"lines\":{Lines},");

            if (!string.IsNullOrEmpty(AlternateLanguage))
                sb.Append($"\"alternate_language\":\"{JsonEscape(AlternateLanguage)}\",");

            if (IsUnsavedEntity)
                sb.Append("\"is_unsaved_entity\":true,");

            sb.Append($"\"category\":\"{Category.GetDescription()}\",");
            sb.Append($"\"entity_type\":\"{EntityType.GetDescription()}\"}}");

            return sb.ToString();
        }

        private static string JsonEscape(string value)
        {
            return (value ?? string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
