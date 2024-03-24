using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class WakaTimeExtension
    {
        public static CliHeartbeat CreateHeartbeat(this WakaTime wakaTime)
        {
            var beat = new CliHeartbeat(wakaTime);
            return beat;
        }
        
        public static CliHeartbeat CreateHeartbeat(this WakaTime wakaTime, string currentFile, bool isWrite, string project,
                                                   HeartbeatCategory? category = null, EntityType? entityType = null)
        {
            var beat = new CliHeartbeat(wakaTime);
            var commonFlags = wakaTime.CommonFlagsHolder.Flags;
            beat.AddFlags(commonFlags.Values);
            
            beat.AddFlagEntity(currentFile);
            beat.AddFlagWrite(isWrite);
            beat.AddFlagProjectAlternate(project);
            if(category.HasValue) beat.AddFlagCategory(category.Value);
            if(entityType.HasValue) beat.AddFlagEntityType(entityType.Value);
            
            
            
            return beat;
        }

        public static WakaTime AddFlag(this WakaTime wakaTime, IFlag flag, bool overwrite = true)
        {
            wakaTime.CommonFlagsHolder.AddFlag(flag, overwrite);
            return wakaTime;
        }
    }
}