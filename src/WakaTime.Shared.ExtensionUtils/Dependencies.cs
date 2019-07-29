using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace WakaTime.Shared.ExtensionUtils
{
    public class Dependencies
    {
        private const string CurrentPythonVersion = "3.6.0";

        private readonly ILogger _logger;

        public Dependencies()
        {
            _logger = new Logger();
        }

        private string PythonBinaryLocation { get; set; }

        private static string PythonDownloadUrl
        {
            get
            {
                var arch = ProcessorArchitectureHelper.Is64BitOperatingSystem ? "amd64" : "win32";
                return string.Format("https://www.python.org/ftp/python/{0}/python-{0}-embed-{1}.zip",
                    CurrentPythonVersion, arch);
            }
        }

        private static string AppDataDirectory
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

        internal string CliLocation => Path.Combine(AppDataDirectory, Constants.CliFolder);

        private string LatestWakaTimeCliVersion()
        {
            var regex = new Regex(@"(__version_info__ = )(\(( ?\'[0-9]+\'\,?){3}\))");

            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            var client = new WebClient { Proxy = new Proxy().Get() };

            try
            {
                var about = client.DownloadString("https://raw.githubusercontent.com/wakatime/wakatime/master/wakatime/__about__.py");
                var match = regex.Match(about);

                if (match.Success)
                {
                    var grp1 = match.Groups[2];
                    var regexVersion = new Regex("([0-9]+)");
                    var match2 = regexVersion.Matches(grp1.Value);
                    return $"{match2[0].Value}.{match2[1].Value}.{match2[2].Value}";
                }

                _logger.Warning("Couldn't auto resolve wakatime cli version");
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
            const string url = Constants.CliUrl;
            var destinationDir = AppDataDirectory;
            var localZipFile = Path.Combine(destinationDir, "wakatime-cli.zip");

            // Download wakatime-cli
            DownloadFile(url, localZipFile);
            _logger.Debug("Finished downloading wakatime-cli.");

            // Remove old folder if it exists
            RecursiveDelete(Path.Combine(destinationDir, "wakatime-master"));

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

        internal void DownloadAndInstallPython()
        {
            _logger.Debug("Downloading python...");
            var url = PythonDownloadUrl;
            var destinationDir = AppDataDirectory;
            var localZipFile = Path.Combine(destinationDir, "python.zip");
            var extractToDir = Path.Combine(destinationDir, "python");

            // Download python
            DownloadFile(url, localZipFile);
            _logger.Debug("Finished downloading python.");

            // Remove old python folder if it exists
            RecursiveDelete(extractToDir);

            // Extract wakatime cli zip file
            _logger.Debug($"Extracting python to: {extractToDir}");
            ZipFile.ExtractToDirectory(localZipFile, extractToDir);
            _logger.Debug("Finished extracting python.");

            try
            {
                File.Delete(localZipFile);
            }
            catch
            { /* ignored */ }
        }

        internal bool IsPythonInstalled()
        {
            return GetPython() != null;
        }

        internal string GetPython()
        {
            if (PythonBinaryLocation == null)
                PythonBinaryLocation = GetEmbeddedPythonPath();

            if (PythonBinaryLocation == null)
                PythonBinaryLocation = GetPythonPathFromMicrosoftRegistry();

            return PythonBinaryLocation ?? (PythonBinaryLocation = GetPythonPathFromFixedPath());
        }

        private string GetPythonPathFromMicrosoftRegistry()
        {
            try
            {
                var regex = new Regex(@"""([^""]*)\\([^""\\]+(?:\.[^"".\\]+))""");
                var pythonKey = Registry.ClassesRoot.OpenSubKey(@"Python.File\shell\open\command");
                if (pythonKey == null)
                    return null;

                var python = pythonKey.GetValue(null).ToString();
                var match = regex.Match(python);

                if (!match.Success) return null;

                var directory = match.Groups[1].Value;
                var fullPath = Path.Combine(directory, "pythonw");
                var process = new RunProcess(fullPath, "--version");

                process.Run();

                if (!process.Success)
                    return null;

                _logger.Debug($"Python found from Microsoft Registry: {fullPath}");

                return fullPath;
            }
            catch (Exception ex)
            {
                _logger.Error("GetPathFromMicrosoftRegistry:", ex);
                return null;
            }
        }

        internal string GetPythonPathFromFixedPath()
        {
            var locations = new List<string>();
            for (var i = 27; i <= 50; i++)
            {
                locations.Add(Path.Combine("\\python" + i, "pythonw"));
                locations.Add(Path.Combine("\\Python" + i, "pythonw"));
            }

            foreach (var location in locations)
            {
                try
                {
                    var process = new RunProcess(location, "--version");
                    process.Run();

                    if (!process.Success) continue;
                }
                catch
                { /*ignored*/ }

                _logger.Debug($"Python found by Fixed Path: {location}");

                return location;
            }

            return null;
        }

        internal string GetEmbeddedPythonPath()
        {
            var path = Path.Combine(AppDataDirectory, "python", "pythonw");
            try
            {
                var process = new RunProcess(path, "--version");
                process.Run();

                if (!process.Success)
                    return null;

                _logger.Debug($"Python found from embedded location: {path}");

                return path;
            }
            catch (Exception ex)
            {
                _logger.Error("GetEmbeddedPath:", ex);
                return null;
            }
        }

        internal bool DoesCliExist()
        {
            return File.Exists(CliLocation);
        }

        internal bool IsCliUpToDate()
        {
            var process = new RunProcess(GetPython(), CliLocation, "--version");
            process.Run();

            if (!process.Success) return false;

            var currentVersion = process.Error.Trim();
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

        internal void DownloadFile(string url, string saveAs)
        {
            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }

            var proxy = new Proxy().Get();
            var client = new WebClient {Proxy = proxy};
            client.DownloadFile(url, saveAs);
        }

        internal void RecursiveDelete(string folder)
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
