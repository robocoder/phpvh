using Components;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhpVH.Tests.Integration
{
    public class TestSettings
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string Webroot { get; set; }

        public bool LaunchInAppDomain { get; set; }

        public bool LogConsoleOutput { get; set; }

        public TestSettings()
        {
            Port = 80;
            Host = "localhost";
            LaunchInAppDomain = false;
            LogConsoleOutput = false;
        }

        public void LoadAppSettings()
        {
            Host = ConfigurationManager.AppSettings["host"];
            Port = int.Parse(ConfigurationManager.AppSettings["port"]);
            LogConsoleOutput = bool.Parse(ConfigurationManager.AppSettings["logConsoleOutput"]);
            Webroot = ConfigurationManager.AppSettings["webRoot"];
            LaunchInAppDomain = bool.Parse(ConfigurationManager.AppSettings["launchInAppDomain"]);
        }

        public static TestSettings FromScanConfig(ScanConfig config)
        {
            return new TestSettings()
            {
                Host = config.Server,
                Port = config.Port,
                Webroot = config.WebRoot,
                LaunchInAppDomain = true,
                LogConsoleOutput = false,
            };
        }

        public static TestSettings Load()
        {
            if (ServiceLocator.Default.IsRegistered<ScanConfig>())
            {
                return FromScanConfig(ServiceLocator.Default.Resolve<ScanConfig>());
            }
            else
            {
                var settings = new TestSettings();
                settings.LoadAppSettings();
                return settings;
            }
        }

        public ScanConfig ToScanConfig()
        {
            return new ScanConfig()
            {
                Server = Host,
                Port = Port,
                WebRoot = Webroot,
            };
        }
    }
}
