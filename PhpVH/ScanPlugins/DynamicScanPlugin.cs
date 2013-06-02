using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PhpVH.ScanPlugins
{
    public class DynamicScanPlugin : ScanPluginBase
    {
        public override int ModeCount
        {
            get { return _badStrings.Length; }
        }

        private readonly string[] _badStrings = new string[] 
        {
            "DynamicClassProbe",
            "DynamicFunctionProbe",
        };

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public DynamicScanPlugin(string Server)
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
            if (TargetTrace.Response.Contains("DynamicClassProbe Instantiated") ||
                Regex.IsMatch(TargetTrace.Response, 
                    @"Class '(" + Php.ValidNameRegex + @")?DynamicClassProbe(" + 
                    Php.ValidNameEndRegex + @")*' not found"))
                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "User Controlled Dynamic Class Instantiation", TargetTrace);
            else if (TargetTrace.Response.Contains("DynamicFunctionProbe Called") ||
                Regex.IsMatch(TargetTrace.Response, 
                    @"Call to undefined function (" + Php.ValidNameRegex + 
                    @")?DynamicFunctionProbe(" + Php.ValidNameEndRegex + @")*"))
                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "User Controlled Dynamic Function Call", TargetTrace);
            else
                return null;
        }

        public override string ToString()
        {
            return "Dynamic Function Call/Class Instantiation Scan";
        }
    }
}
