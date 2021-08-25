namespace WakaTime.Shared.ExtensionUtils
{
    public static class Constants
    {
        internal const string S3UrlPrefix = "https://wakatime-cli.s3-us-west-2.amazonaws.com/";
        internal const string GithubDownloadPrefix = "https://github.com/wakatime/wakatime-cli/releases/download";
        internal const string GithubReleasesAlphaUrl = "https://api.github.com/repos/wakatime/wakatime-cli/releases?per_page=1";
        internal const string GithubReleasesStableUrl = "https://api.github.com/repos/wakatime/wakatime-cli/releases/latest";

        internal const int HeartbeatFrequency = 2; // minutes
    }
}
