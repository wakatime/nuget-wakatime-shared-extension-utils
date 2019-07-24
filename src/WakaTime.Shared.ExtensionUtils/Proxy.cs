using System;
using System.Net;
using System.Text.RegularExpressions;

namespace WakaTime.Shared.ExtensionUtils
{
    public class Proxy
    {
        private readonly ConfigFile _config;

        public Proxy()
        {
            _config = new ConfigFile();
            _config.Read();
        }

        public WebProxy Get()
        {
            WebProxy proxy = null;

            try
            {
                if (string.IsNullOrEmpty(_config.Proxy))
                {
                    Logger.Debug("No proxy will be used. It's either not set or badly formatted.");
                    return null;
                }

                var proxyStr = _config.Proxy;

                // Regex that matches proxy address with authentication
                var regProxyWithAuth = new Regex(@"\s*(https?:\/\/)?([^\s:]+):([^\s:]+)@([^\s:]+):(\d+)\s*");
                var match = regProxyWithAuth.Match(proxyStr);

                if (match.Success)
                {
                    var username = match.Groups[2].Value;
                    var password = match.Groups[3].Value;
                    var address = match.Groups[4].Value;
                    var port = match.Groups[5].Value;

                    var credentials = new NetworkCredential(username, password);
                    proxy = new WebProxy(string.Join(":", address, port), true, null, credentials);

                    Logger.Debug("A proxy with authentication will be used.");
                    return proxy;
                }

                // Regex that matches proxy address and port(no authentication)
                var regProxy = new Regex(@"\s*(https?:\/\/)?([^\s@]+):(\d+)\s*");
                match = regProxy.Match(proxyStr);

                if (match.Success)
                {
                    var address = match.Groups[2].Value;
                    var port = int.Parse(match.Groups[3].Value);

                    proxy = new WebProxy(address, port);

                    Logger.Debug("A proxy will be used.");
                    return proxy;
                }

                Logger.Debug("No proxy will be used. It's either not set or badly formatted.");
            }
            catch (Exception ex)
            {
                Logger.Error(
                    "Exception while parsing the proxy string from WakaTime config file. No proxy will be used.",
                    ex);
            }

            return proxy;
        }
    }
}
