using System;
using WakaTime.Shared.ExtensionUtils.Flags;

namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class WakaTimeExtension
    {
        /// <summary>
        ///     Creates a new heartbeat with common flags from <see cref="WakaTime" />.<see cref="WakaTime.CommonFlags" /> added by
        ///     default. <br />
        ///     Also, adds <see cref="FlagCategory" /> and <see cref="FlagEntityType" /> flags with default values and
        ///     <see cref="FlagTime" /> flag with current time. <br />
        ///     <br /> <b>NOTE:</b> This method does not add <see cref="FlagEntity" /> flag.
        ///     Use <see cref="CreateHeartbeat(WakaTime,string)" /> to add the current file path.
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        /// <returns>
        ///     The <see cref="FlagHolder" /> instance representing the heartbeat. You can continue managing flags with the
        ///     <c>AddFlag*</c> and <c>RemoveFlag*</c> methods.
        ///     Also, you can use <see cref="FlagHolderExtension.Send" /> method to send the heartbeat to the WakaTime API.
        /// </returns>
        /// <example>
        /// <code>
        /// var myWakaTime = new WakaTime(...);
        /// var heartbeat = myWakaTime.CreateHeartbeat();
        /// heartbeat.AddFlagEntity("path/to/file");
        /// heartbeat.AddFlagWrite(true);
        /// heartbeat.Send();
        ///
        /// // OR
        /// heartbeat = myWakaTime.CreateHeartbeat().AddFlagEntity("path/to/file").AddFlagWrite(true);
        /// heartbeat.Send();
        ///
        /// // OR
        /// myWakaTime.CreateHeartbeat().AddFlagEntity("path/to/file").AddFlagWrite(true).Send();
        ///
        /// // OR
        /// heartbeat = myWakaTime.CreateHeartbeat().AddFlagEntity("path/to/file").AddFlagWrite(true);
        /// myWakaTime.HandleActivity(heartbeat);
        /// </code>
        /// </example>
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


        /// <summary>
        ///     Creates a new heartbeat with common flags from <see cref="WakaTime" />.<see cref="WakaTime.CommonFlags" /> added by
        ///     default. <br />
        ///     Also, adds <see cref="FlagCategory" /> and <see cref="FlagEntityType" /> flags with default values and
        ///     <see cref="FlagTime" /> flag with current time. <br />
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        /// <param name="currentFile">
        ///     The current file path. This will be added as <see cref="FlagEntity" /> flag, which is
        ///     required for the heartbeat.
        /// </param>
        /// <returns>
        ///     The <see cref="FlagHolder" /> instance representing the heartbeat. You can continue managing flags with the
        ///     <c>AddFlag*</c> and <c>RemoveFlag*</c> methods.
        ///     Also, you can use <see cref="FlagHolderExtension.Send" /> method to send the heartbeat to the WakaTime API.
        /// </returns>
        /// <example>
        ///     For example see <see cref="CreateHeartbeat(WakaTime)" />.
        /// </example>
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime, string currentFile)
        {
            var beat = CreateHeartbeat(wakaTime);
            beat.AddFlagEntity(currentFile);
            return beat;
        }

        /// <summary>
        ///     Creates a new heartbeat with common flags from <see cref="WakaTime" />.<see cref="WakaTime.CommonFlags" /> added by
        ///     default. <br />
        ///     Also, adds <see cref="FlagCategory" /> and <see cref="FlagEntityType" /> flags with default values and
        ///     <see cref="FlagTime" /> flag with current time. <br />
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        /// <param name="currentFile">
        ///     The current file path. This will be added as <see cref="FlagEntity" /> flag, which is
        ///     required for the heartbeat.
        /// </param>
        /// <param name="isWrite">
        ///     When set, tells api this heartbeat was triggered from writing to a file. This will be added as
        ///     <see cref="FlagWrite" /> flag.
        /// </param>
        /// <returns>
        ///     The <see cref="FlagHolder" /> instance representing the heartbeat. You can continue managing flags with the
        ///     <c>AddFlag*</c> and <c>RemoveFlag*</c> methods.
        ///     Also, you can use <see cref="FlagHolderExtension.Send" /> method to send the heartbeat to the WakaTime API.
        /// </returns>
        /// <example>
        ///     For example see <see cref="CreateHeartbeat(WakaTime)" />.
        /// </example>
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime, string currentFile, bool isWrite)
        {
            var beat = CreateHeartbeat(wakaTime, currentFile);
            beat.AddFlagWrite(isWrite);
            return beat;
        }

        /// <summary>
        ///     Creates a new heartbeat with common flags from <see cref="WakaTime" />.<see cref="WakaTime.CommonFlags" /> added by
        ///     default. <br />
        ///     Also, adds <see cref="FlagCategory" /> and <see cref="FlagEntityType" /> flags with default values and
        ///     <see cref="FlagTime" /> flag with current time. <br />
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        /// <param name="currentFile">
        ///     The current file path. This will be added as <see cref="FlagEntity" /> flag, which is
        ///     required for the heartbeat.
        /// </param>
        /// <param name="isWrite">
        ///     When set, tells api this heartbeat was triggered from writing to a file. This will be added as
        ///     <see cref="FlagWrite" /> flag.
        /// </param>
        /// <param name="alternateProject">
        ///     Alternate project name. This will be added as <see cref="FlagProjectAlternate" /> flag.
        ///     To force the project name flag see <see cref="FlagProject" />.
        /// </param>
        /// <returns>
        ///     The <see cref="FlagHolder" /> instance representing the heartbeat. You can continue managing flags with the
        ///     <c>AddFlag*</c> and <c>RemoveFlag*</c> methods.
        ///     Also, you can use <see cref="FlagHolderExtension.Send" /> method to send the heartbeat to the WakaTime API.
        /// </returns>
        /// <example>
        ///     For example see <see cref="CreateHeartbeat(WakaTime)" />.
        /// </example>
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime, string currentFile, bool isWrite, string alternateProject)
        {
            var beat = CreateHeartbeat(wakaTime, currentFile, isWrite);
            beat.AddFlagProjectAlternate(alternateProject);
            return beat;
        }

        /// <summary>
        ///     Creates a new heartbeat with common flags from <see cref="WakaTime" />.<see cref="WakaTime.CommonFlags" /> added by
        ///     default. <br />
        ///     Also, adds <see cref="FlagCategory" /> and <see cref="FlagEntityType" /> flags with default values and
        ///     <see cref="FlagTime" /> flag with current time. <br />
        /// </summary>
        /// <param name="wakaTime">The <see cref="WakaTime" /> instance.</param>
        /// <param name="currentFile">
        ///     The current file path. This will be added as <see cref="FlagEntity" /> flag, which is
        ///     required for the heartbeat.
        /// </param>
        /// <param name="isWrite">
        ///     When set, tells api this heartbeat was triggered from writing to a file. This will be added as
        ///     <see cref="FlagWrite" /> flag.
        /// </param>
        /// <param name="alternateProject">
        ///     Alternate project name. This will be added as <see cref="FlagProjectAlternate" /> flag.
        ///     To force the project name flag see <see cref="FlagProject" />.
        /// </param>
        /// <param name="category">The category of the heartbeat. This will be added as <see cref="FlagCategory" /> flag.</param>
        /// <param name="entityType">The entity type of the heartbeat. This will be added as <see cref="FlagEntityType" /> flag.</param>
        /// <returns>
        ///     The <see cref="FlagHolder" /> instance representing the heartbeat. You can continue managing flags with the
        ///     <c>AddFlag*</c> and <c>RemoveFlag*</c> methods.
        ///     Also, you can use <see cref="FlagHolderExtension.Send" /> method to send the heartbeat to the WakaTime API.
        /// </returns>
        /// <example>
        ///     For example see <see cref="CreateHeartbeat(WakaTime)" />.
        /// </example>
        public static FlagHolder CreateHeartbeat(this WakaTime wakaTime,
                                                 string currentFile,
                                                 bool isWrite,
                                                 string alternateProject,
                                                 // ReSharper disable once MethodOverloadWithOptionalParameter
                                                 HeartbeatCategory? category = null,
                                                 EntityType? entityType = null)
        {
            var beat = CreateHeartbeat(wakaTime, currentFile, isWrite, alternateProject);
            if (category.HasValue) beat.AddFlagCategory(category.Value);
            if (entityType.HasValue) beat.AddFlagEntityType(entityType.Value);
            return beat;
        }
    }
}