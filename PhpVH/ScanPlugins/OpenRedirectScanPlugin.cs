using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhpVH.ScanPlugins
{
    public class OpenRedirectScanPlugin : ScanPluginBase
    {
        private string[] _badChars = new[] 
        {
            "test.com"
        };

        public override int ModeCount
        {
            get { return _badChars.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public OpenRedirectScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {

            return RequestBuilder.CreateRequest(TargetFile, Server,
                _badChars[Mode], true, false, false);
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            var header = TargetTrace.Response;
            
            var headerIndex = header.IndexOf("\r\n\r\n");

            if (headerIndex != -1)
                header = header.Remove(headerIndex);

            if (Regex.IsMatch(header, @"Location:\s*([^/]+://)?[^/]*" + _badChars[0],
                RegexOptions.IgnoreCase))
            {
                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Open Redirect", TargetTrace);
            }

            return null;
        }

        public override string ToString()
        {
            return "Open Redirect Scan";
        }
    }
}
