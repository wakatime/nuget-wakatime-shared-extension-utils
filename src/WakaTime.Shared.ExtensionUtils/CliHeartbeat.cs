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
            this.AddFlagCategory();
            this.AddFlagEntityType();
        }

        #endregion

        public bool IsValid()
        {
            bool hasEntity = HasFlag(FlagEntity.CliFlagName);
            bool hasEntityType = HasFlag(FlagEntityType.CliFlagName);
            bool hasTime = HasFlag(FlagTime.CliFlagName);
            bool hasCategory = HasFlag(FlagCategory.CliFlagName);

            if (!hasEntity) WakaTime.Logger.Error("Entity is required for sending heartbeat.");
            if(!hasEntityType) WakaTime.Logger.Error("Entity type is required for sending heartbeat.");
            if(!hasTime) WakaTime.Logger.Error("Time is required for sending heartbeat.");
            if(!hasCategory) WakaTime.Logger.Error("Category is required for sending heartbeat.");

            return hasEntity && hasEntityType && hasTime && hasCategory;
        }

        private bool HasFlag(string cliFlagName) => Flags.ContainsKey(cliFlagName);
    }
}