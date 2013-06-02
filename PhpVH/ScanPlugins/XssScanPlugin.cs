using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace PhpVH.ScanPlugins
{
    class XssScanPlugin : ScriptableScanPluginBase<XssScanConfig>
    {
        public override int ModeCount
        {
            get { return Config.FuzzStrings.Length * 2; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public XssScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override void InitializeCore()
        {
            Config = new XssScanConfig() { FuzzStrings = ScriptLauncher.LoadTestCases(ScriptResult) };
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            string chars;

            if (Mode < Config.FuzzStrings.Length)
            {
                chars = Config.FuzzStrings[Mode];
            }
            else
            {
                chars = "\x00" + Config.FuzzStrings[Mode - (ModeCount / 2)];
            }
            
            return RequestBuilder.CreateRequest(TargetFile, Server,
                chars, false, true, true);
        }

        private bool HasAttributeVulnerability(string Response)
        {
            for (int x = 2; x < 4; x++)
            {
                var index = -1;

                var charsLowered = Config.FuzzStrings[x].ToLower();

                while ((index = Response.IndexOf(charsLowered, index + 1)) != -1)
                {
                    for (int i = index; i >= index - 100 && i >= 0; i--)
                    {
                        if (Response[i] == '<')
                            return true;
                        else if (Response[i] == '>')
                            break;
                    }
                }
            }

            return false;
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            var respLowered = TargetTrace.Response.ToLower();

            if ((respLowered.Contains(Config.FuzzStrings[0].ToLower()) ||
                HasAttributeVulnerability(respLowered)) &&
                Regex.IsMatch(respLowered, @"^http/\d\.\d\s200\s"))
            {
                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Reflected XSS", TargetTrace);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return "Reflected Cross-site Scripting Scan";
        }
    }
}
