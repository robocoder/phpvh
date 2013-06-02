using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PhpVH.ScanPlugins
{
    public class ArbitraryPhpScanPlugin : ConfigurableScanPluginBase<ArbitraryPhpScanConfig>
    {
        public override int ModeCount
        {
            get { return Config.FuzzStrings.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public ArbitraryPhpScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override void InitializeCore()
        {
            
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            return RequestBuilder.CreateRequest(TargetFile, Server,
                Config.FuzzStrings[Mode], false, false, true);
        }

        private ScanAlert CreateAlert(FileTrace TargetTrace)
        {
            return new ScanAlert(ScanAlertOptions.Vulnerability,
                "Arbitrarty PHP Execution", TargetTrace);
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            if (TargetTrace.Calls
                .Where(x => x.Name == "eval" && x.ParameterValues.Any(y => y.Contains("testabc")))
                .Any())
                return CreateAlert(TargetTrace);

            var falsePositiveRegex = new Regex(Config.FalsePositiveRegex);

            var Response = falsePositiveRegex.Replace(TargetTrace.Response, "");

            var regex = new Regex(Config.MatchRegex);

            if (regex.IsMatch(Response))
                return CreateAlert(TargetTrace);
            else
                return null;
        }

        public override string ToString()
        {
            return "Arbitrary PHP Execution Scan";
        }
    }
}
