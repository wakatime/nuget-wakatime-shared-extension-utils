using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace WakaTime.Shared.ExtensionUtils
{
    public class Dependencies
    {
        private readonly ILogger _logger;
        private readonly ConfigFile _configFile;
        private readonly bool _newBetaCli;

        public Dependencies(ILogger logger, ConfigFile configFile)
        {
            _logger = logger;
            _configFile = configFile;
            _newBetaCli = _configFile.GetSettingAsBoolean("new_beta_cli", true);
        }

        public static string HomeLocation
        {
            get
            {
                var wakaHome = Environment.GetEnvironmentVariable("WAKATIME_HOME");

                if (!string.IsNullOrEmpty(wakaHome) && Directory.Exists(wakaHome))
                    return wakaHome;

                return Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            }
        }

        public static string ResourcesLocation
        {
            get
            {
                var path = Path.Combine(HomeLocation, ".wakatime");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path;
            }
        }

        public async Task CheckAndInstall()
        {
            if (_newBetaCli)
                await CheckAndInstallCli();
            else
                await CheckAndInstallLegacyCli();
        }

        private async Task CheckAndInstallCli()
        {
            if (!IsCliInstalled(true) || !await IsCliLatest())
                await InstallCli();
        }

        private async Task CheckAndInstallLegacyCli()
        {
            if (!IsCliInstalled(false) || !await IsLegacyCliLatest())
                await InstallLegacyCli();
        }

        private async Task InstallCli()
        {
            var version = await GetLatestCliVersion();

            _logger.Debug($"Downloading wakatime-cli {version}...");

            var arch = ProcessorArchitectureHelper.Is64BitOperatingSystem ? "amd64" : "386";
            var url = $"{Constants.GithubDownloadPrefix}/{version}/wakatime-cli-windows-{arch}.zip";
            var localZipFile = Path.Combine(ResourcesLocation, $"wakatime-cli-{version}.zip");

            // Download wakatime-cli
            await DownloadFile(url, localZipFile);
            _logger.Debug($"Finished downloading wakatime-cli {version}");

            // Extract wakatime-cli zip file
            _logger.Debug($"Extracting wakatime-cli to: {ResourcesLocation}");
            using (var archive = ZipFile.OpenRead(localZipFile))
            {
                foreach (var entry in archive.Entries)
                {
                    entry.ExtractToFile(Path.Combine(ResourcesLocation, entry.FullName), true);
                }
            }
            _logger.Debug("Finished extracting wakatime-cli");

            try
            {
                File.Delete(localZipFile);
            }
            catch
            { /* ignored */ }
        }

        private async Task InstallLegacyCli()
        {
            _logger.Debug("Downloading legacy python wakatime-cli...");

            var url = $"{GetS3BucketUrl()}/wakatime-cli.zip";

            var localZipFile = Path.Combine(ResourcesLocation, "wakatime-cli.zip");

            // Download wakatime-cli
            await DownloadFile(url, localZipFile);
            _logger.Debug($"Finished downloading wakatime-cli");

            // Remove old folder if it exists
            RecursiveDelete(Path.Combine(ResourcesLocation, "wakatime-cli"));

            // Extract wakatime-cli zip file
            _logger.Debug($"Extracting wakatime-cli to: {ResourcesLocation}");
            using (var archive = ZipFile.OpenRead(localZipFile))
            {
                foreach (var entry in archive.Entries)
                {
                    entry.ExtractToFile(Path.Combine(ResourcesLocation, entry.FullName), true);
                }
            }
            _logger.Debug("Finished extracting wakatime-cli");

            try
            {
                File.Delete(localZipFile);
            }
            catch
            { /* ignored */ }
        }

        private async Task<string> GetLatestCliVersion()
        {
            try
            {
                var isAlpha = _configFile.GetSettingAsBoolean("alpha");
                var url = isAlpha
                    ? Constants.GithubReleasesAlphaUrl
                    : Constants.GithubReleasesStableUrl;

                var client = GetHttpClient();
                var req = new HttpRequestMessage(HttpMethod.Get, url);
                req.Headers.TryAddWithoutValidation("User-Agent", "github.com/wakatime/visualstudio-wakatime");

                var cliVersionLastModified = _configFile.GetSetting("cli_version_last_modified", "internal");
                if (!string.IsNullOrEmpty(cliVersionLastModified))
                    req.Headers.TryAddWithoutValidation("If-Modified-Since", cliVersionLastModified);

                var res = await client.SendAsync(req);

                _logger.Debug($"GitHub API Response {res?.StatusCode}");

                if (res.StatusCode == HttpStatusCode.NotModified)
                    return _configFile.GetSetting("cli_version", "internal");

                var resBody = await res.Content.ReadAsStringAsync();
                string version;

                if (isAlpha)
                    version = JSONSerializer.DeSerialize<IList<GithubReleaseApiResponse>>(resBody)[0].TagName;
                else
                    version = JSONSerializer.DeSerialize<GithubReleaseApiResponse>(resBody).TagName;

                _logger.Debug($"Latest wakatime-cli version from GitHub: {version}");

                res.Headers.TryGetValues("Last-Modified", out var lastModifiedList);
                var lastModified = lastModifiedList?.FirstOrDefault();
                if (!string.IsNullOrEmpty(lastModified))
                {
                    _configFile.SaveSetting("internal", "cli_version", version);
                    _configFile.SaveSetting("internal", "cli_version_last_modified", lastModified);
                }

                return version;

            }
            catch (Exception ex)
            {
                _logger.Error("Error getting latest wakatime cli version from GitHub", ex);
            }

            return null;
        }

        private async Task<string> GetLatestLegacyCliVersion()
        {
            try
            {
                var url = $"{GetS3BucketUrl()}/current_version.txt";
                var client = GetHttpClient();
                var version = await client.GetStringAsync(url);

                return version.Trim();

            }
            catch (Exception ex)
            {
                _logger.Error("Error getting latest legacy wakatime cli version from S3", ex);
            }

            return null;
        }

        private async Task<bool> IsCliLatest()
        {
            var process = new RunProcess(GetCliLocation(true), "--version");

            process.Run();

            if (!process.Success)
            {
                _logger.Error(process.Error);

                return false;
            }

            var currentVersion = process.Output.Trim();

            _logger.Debug($"Current wakatime-cli version is {currentVersion}");
            _logger.Debug("Checking for updates to wakatime-cli...");

            var latestVersion = await GetLatestCliVersion();

            if (currentVersion.Equals(latestVersion))
            {
                _logger.Info("wakatime-cli is up to date");
                return true;
            }

            _logger.Info($"Found an updated wakatime-cli {latestVersion}");

            return false;
        }

        private async Task<bool> IsLegacyCliLatest()
        {
            var process = new RunProcess(GetCliLocation(false), "--version");

            process.Run();

            if (!process.Success)
            {
                _logger.Error(process.Error);

                return false;
            }

            var currentVersion = process.Output.Trim();
            _logger.Debug($"Current wakatime-cli version is {currentVersion}");
            _logger.Debug("Checking for updates to wakatime-cli...");

            var latestVersion = await GetLatestLegacyCliVersion();

            if (currentVersion.Equals(latestVersion))
            {
                _logger.Info("wakatime-cli is up to date");
                return true;
            }

            _logger.Info($"Found an updated wakatime-cli {latestVersion}");

            return false;
        }

        private static string GetS3BucketUrl()
        {
            return Path.Combine(Constants.S3UrlPrefix,
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                    ? $"windows-x86-{(ProcessorArchitectureHelper.Is64BitOperatingSystem ? "64" : "32")}"
                    : "mac-x86-64");
        }

        public static string GetConfigFilePath()
        {
            return Path.Combine(HomeLocation, ".wakatime.cfg");
        }

        internal string GetCliLocation(bool newGoCli)
        {
            var ext = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : "";

            if (!newGoCli)
                return Path.Combine(ResourcesLocation, "wakatime-cli", $"wakatime-cli{ext}");

            var arch = ProcessorArchitectureHelper.Is64BitOperatingSystem ? "amd64" : "386";

            return Path.Combine(ResourcesLocation, $"wakatime-cli-windows-{arch}{ext}");
        }

        private bool IsCliInstalled(bool newGoCli)
        {
            return File.Exists(GetCliLocation(newGoCli));
        }

        private HttpClient GetHttpClient()
        {
            var handler = new HttpClientHandler();

            if (!_configFile.GetSettingAsBoolean("no_ssl_verify") &&
                !ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }

            var proxy = _configFile.GetSetting("proxy");
            if (!string.IsNullOrEmpty(proxy))
            {
                handler.UseProxy = true;
                handler.Proxy = new Proxy(_logger, _configFile.ConfigFilepath).Get();
            }

            return new HttpClient(handler);
        }

        private async Task DownloadFile(string url, string saveAs)
        {
            var client = GetHttpClient();

            var res = await client.GetAsync(url);
            var stream = await res.Content.ReadAsStreamAsync();

            using (var fileStream = File.Create(saveAs))
                stream.CopyTo(fileStream);
        }

        private void RecursiveDelete(string folder)
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
