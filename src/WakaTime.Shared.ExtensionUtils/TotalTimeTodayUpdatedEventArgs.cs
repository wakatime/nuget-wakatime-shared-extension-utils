using System;

namespace WakaTime.Shared.ExtensionUtils
{
    public class TotalTimeTodayUpdatedEventArgs : EventArgs
    {
        public string TotalTimeToday { get; }

        public TotalTimeTodayUpdatedEventArgs(string totalTimeToday)
        {
            TotalTimeToday = totalTimeToday;
        }
    }
}
