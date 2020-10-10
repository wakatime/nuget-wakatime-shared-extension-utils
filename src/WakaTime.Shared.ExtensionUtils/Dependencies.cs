using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.InteropServices;

namespace WakaTime.Shared.ExtensionUtils
{
    public class Dependencies
    {
        private readonly ILogger _logger;

        public Dependencies()
        {
            _logger = new Logger();
        }

        public static string AppDataDirectory
        {
            get
            {
                var roamingFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var appFolder = Path.Combine(roamingFolder, "WakaTime");

                // Create folder if it does not exist
                if (!Directory.Exists(appFolder))
                    Directory.CreateDirectory(appFolder);

                return appFolder;
            }
        }

        // ReSharper disable once MemberCanBeMadeStatic.Global
        internal string CliLocation => Path.Combine(AppDataDirectory, Constants.CliFolder);

        private string LatestWakaTimeCliVersion()
        {
            try
            {
                return DownloadString(Path.Combine(GetS3BucketUrl(), "current_version.txt"));
            }
            catch (Exception ex)
            {
                _logger.Error("Exception when checking current wakatime cli version: ", ex);
            }

            return string.Empty;
        }

        internal void DownloadAndInstallCli()
        {
            _logger.Debug("Downloading wakatime-cli...");
            var url = Path.Combine(GetS3BucketUrl(), "wakatime-cli.zip");
            var destinationDir = AppDataDirectory;
            var localZipFile = Path.Combine(destinationDir, "wakatime-cli.zip");

            // Download wakatime-cli
            DownloadFile(url, localZipFile);
            _logger.Debug("Finished downloading wakatime-cli.");

            // Remove old folder if it exists
            RecursiveDelete(Path.Combine(destinationDir, "wakatime-cli"));

            // Extract wakatime-cli zip file
            _logger.Debug($"Extracting wakatime-cli to: {destinationDir}");
            ZipFile.ExtractToDirectory(localZipFile, destinationDir);
            _logger.Debug("Finished extracting wakatime-cli.");

            try
            {
                File.Delete(localZipFile);
            }
            catch
            { /* ignored */ }
        }
        private static string GetS3BucketUrl()
        {
            return Path.Combine(Constants.S3UrlPrefix,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? $"windows-x86-{(ProcessorArchitectureHelper.Is64BitOperatingSystem ? "64" : "32")}"
                    : "mac-x86-64");
        }

        internal bool DoesCliExist()
        {
            return File.Exists(CliLocation);
        }

        internal bool IsCliUpToDate()
        {
            var process = new RunProcess(CliLocation, "--version");

            process.Run();
            if (!process.Success)
            {
                _logger.Error(process.Error);
                return false;
            }

            var currentVersion = process.Output.Trim();
            _logger.Info($"Current wakatime-cli version is {currentVersion}");
            _logger.Info("Checking for updates to wakatime-cli...");

            var latestVersion = LatestWakaTimeCliVersion();

            if (currentVersion.Equals(latestVersion))
            {
                _logger.Info("wakatime-cli is up to date.");
                return true;
            }

            _logger.Info($"Found an updated wakatime-cli v{latestVersion}");

            return false;
        }

        private static WebClient GetWebClient()
        {
            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            var proxy = new Proxy().Get();
            return new WebClient { Proxy = proxy };
        }

        private static void DownloadFile(string url, string saveAs)
        {
            var client = GetWebClient();
            client.DownloadFile(url, saveAs);
        }

        private static string DownloadString(string url)
        {
            var client = GetWebClient();
            return client.DownloadString(url).Trim();
        }

        private static void RecursiveDelete(string folder)
        {
            try
            {
                Directory.Delete(folder, true);
            }
            catch
            { /* ignored */ }

            try
            {
                File.Delete(folder);
            }
            catch
            { /* ignored */ }
        }
    }
}
