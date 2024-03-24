using System;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils
{
    public class CliHeartbeat : FlagHolder
    {
        #region Constructors

        public CliHeartbeat(WakaTime wakaTime) : base(wakaTime)
        {
            this.AddFlagTime(DateTime.UtcNow);
            this.AddFlagCategory(HeartbeatCategory.Coding);
            this.AddFlagEntityType(EntityType.File);
        }

        #endregion
    }
}