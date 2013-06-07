using Components;
using Components.ConsolePlus;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PhpVH
{
    public sealed class PhpVersionTester
    {
        private ScanConfig _config;

        private FileInfo _probeFile = null;

        private string[] _knownVersions = new[]
        {
            "5.3.1",
            "5.3.5",
            "5.3.8",
            "5.4.7",
        };

        public bool ThrowOnFail { get; set; }

        public PhpVersionTester(ScanConfig config)
        {
            _config = config;
        }

        private void WriteProbe()
        {
            var name = _config.WebRoot + "\\VersionProbe.php";

            Cli.WriteLine("Writing probe to ~Cyan~{0}~R~", name);

            File.WriteAllText(name, PhpResource.Load("VersionProbe"));
            _probeFile = new FileInfo(name);
        }

        private void DeleteProbe()
        {
            _probeFile.Refresh();
            if (_probeFile != null)
            {
                _probeFile.Delete();
            }
        }

        private string RequestProbe()
        {
            var url = string.Format("http://{0}:{1}/{2}", _config.Server, _config.Port, _probeFile.Name);

            Cli.WriteLine("Requesting ~Cyan~{0}~R~", url);
            
            try
            {
                return new WebClient().DownloadString(url);
            }
            catch (WebException)
            {
                if (ThrowOnFail)
                {
                    throw;
                }

                return null;
            }
        }

        public void CheckVersion()
        {
            try
            {
                WriteProbe();

                var resp = RequestProbe();

                var isVersion = false;

                if (resp != null)
                {
                    isVersion = Regex.IsMatch(resp, @"^[0-9.]+$");
                }

                if (isVersion)
                {
                    Cli.WriteLine("PHP version ~Cyan~{0}~R~ detected", resp);

                    if (_knownVersions.Contains(resp))
                    {
                        Cli.WriteLine("~Green~Known PHP version~R~");
                    }
                    else
                    {
                        Cli.WriteLine("~Yellow~Unknown PHP version; PhpVH may not work properly on untested versions~R~");
                    }
                }
                else if (resp == null)
                {
                    if (ThrowOnFail)
                    {
                        throw new InvalidOperationException("No response from server"); 
                    }
                    else
                    {
                        ScannerCli.DisplayCriticalMessageAndExit("~Red~No response from server; exiting~R~");
                    }
                }
                else
                {
                    var maxRespLen = 60;

                    if (resp.Length > maxRespLen)
                    {
                        resp = resp.Remove(maxRespLen);
                    }

                    if (ThrowOnFail)
                    {
                        throw new InvalidOperationException(string.Format("PHP version check failed~R~\r\nProbe response: {0}", resp));
                    }

                    ScannerCli.DisplayCriticalMessageAndExit(
                        "~Red~PHP version check failed~R~\r\nProbe response: {0}\r\nThis error generally occurs when the webroot is not properly configured.",
                        maxRespLen);
                }
            }
            finally
            {
                DeleteProbe();
            }
        }
    }
}
