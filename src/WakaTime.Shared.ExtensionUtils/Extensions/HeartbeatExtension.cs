namespace WakaTime.Shared.ExtensionUtils.Extensions
{
    public static class HeartbeatExtension
    {
        /// <summary>
        ///     Sends the heartbeat to the WakaTime API.
        /// </summary>
        /// <param name="heartbeat">The <see cref="FlagHolder" /> instance representing the heartbeat.</param>
        /// <param name="throwException">Whether to throw an exception if the heartbeat is not valid, i.e. has missing required flags.</param>
        /// <seealso cref="Heartbeat.IsValid" />
        public static void Send(this Heartbeat heartbeat, bool throwException = false)
        {
            heartbeat.WakaTime.HandleActivity(heartbeat, throwException);
        }
    }
}