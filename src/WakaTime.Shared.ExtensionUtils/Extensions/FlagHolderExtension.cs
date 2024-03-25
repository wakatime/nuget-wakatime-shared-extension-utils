namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class FlagHolderExtension
    {
        /// <summary>
        ///     Sends the heartbeat to the WakaTime API.
        /// </summary>
        /// <param name="heartbeat">The <see cref="FlagHolder" /> instance representing the heartbeat.</param>
        /// <param name="throwException">Whether to throw an exception if the heartbeat is not valid, i.e. has missing required flags.</param>
        /// <seealso cref="FlagHolder.IsValidHeartbeat" />
        public static void Send(this FlagHolder heartbeat, bool throwException = false)
        {
            heartbeat.WakaTime.HandleActivity(heartbeat, throwException);
        }
    }
}