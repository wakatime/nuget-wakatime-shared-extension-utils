using System;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class WakaTimeExtension
    {
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime)
        {
            var beat = new FlagHolder(wakaTime);
            var commonFlags = wakaTime.CommonFlags.Flags;
            beat.AddFlags(commonFlags.Values);
            
            beat.AddFlagCategory();
            beat.AddFlagEntityType();
            beat.AddFlagTime(DateTime.UtcNow);
            
            return beat;
        }
        
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime, string currentFile, bool isWrite, string project,
                                                 HeartbeatCategory? category = null, EntityType? entityType = null)
        {
            var beat = CreateHeartbeat(wakaTime);
            
            if (category.HasValue) beat.AddFlagCategory(category.Value);
            beat.AddFlagEntity(currentFile);
            
            if(entityType.HasValue) beat.AddFlagEntityType(entityType.Value);
                
            beat.AddFlagProjectAlternate(project);
            beat.AddFlagWrite(isWrite);
            
            return beat;
        }
    }
}