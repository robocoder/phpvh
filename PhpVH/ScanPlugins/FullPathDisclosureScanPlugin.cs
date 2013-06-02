using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhpVH.ScanPlugins
{
    public class FullPathDisclosureScanPlugin : ScanPluginBase
    {
        private string[] _badStrings = new[] 
        {
            "\x00",
            ""
        };

        public override int ModeCount
        {
            get { return _badStrings.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public FullPathDisclosureScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            
            return RequestBuilder.CreateRequest(TargetFile, Server,
                _badStrings[Mode], false, false, true);
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            return Regex.IsMatch(TargetTrace.Response,
                @"[^\w][^\\/:*?""<>|]:([\\/]+[^\\/:*?""<>|]+)+") ?
                    new ScanAlert(ScanAlertOptions.Vulnerability,
                        "Full Path Disclosure", TargetTrace) :
                    null;
        }

        public override string ToString()
        {
            return "Full Path Disclosure Scan";
        }
    }
}
