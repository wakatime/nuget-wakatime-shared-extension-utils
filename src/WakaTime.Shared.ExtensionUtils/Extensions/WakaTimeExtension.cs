using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class WakaTimeExtension
    {
        public static CliHeartbeat CreateHeartbeat(this WakaTime wakaTime)
        {
            var beat = new CliHeartbeat(wakaTime);
            foreach (var flag in wakaTime.CommonFlagsHolder.Flags.Values) beat.AddFlag(flag);
            return beat;
        }
        
        public static CliHeartbeat CreateHeartbeat(this WakaTime wakaTime, string currentFile, bool isWrite, string project,
                                                   HeartbeatCategory? category = null, EntityType? entityType = null)
        {
            var beat = new CliHeartbeat(wakaTime);
            foreach (var flag in wakaTime.CommonFlagsHolder.Flags.Values) beat.AddFlag(flag);
            
            if(category.HasValue) beat.AddFlagCategory(category.Value);
            
            return beat;
        }

        public static WakaTime AddFlag(this WakaTime wakaTime, ICliFlag flag, bool overwrite = true)
        {
            wakaTime.CommonFlagsHolder.AddFlag(flag, overwrite);
            return wakaTime;
        }
    }
}