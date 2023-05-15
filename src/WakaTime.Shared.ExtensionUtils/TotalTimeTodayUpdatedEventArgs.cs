using System;

namespace WakaTime.Shared.ExtensionUtils
{
    public class TotalTimeTodayUpdatedEventArgs : EventArgs
    {
        public string TotalTimeToday { get; }
        public string TotalTimeTodayDetailed { get; }

        public TotalTimeTodayUpdatedEventArgs(string totalTimeToday, string totalTimeTodayDetailed)
        {
            TotalTimeToday = totalTimeToday;
            TotalTimeTodayDetailed = totalTimeTodayDetailed;
        }
    }
}
