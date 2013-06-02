using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PhpVH.ScanPlugins
{
    public class SqlScanPlugin : ConfigurableScanPluginBase<SqlScanConfig>
    {
        private static string[] _anchors = new[] { "testab", "testxy" };        

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

        public SqlScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override void InitializeCore()
        {
            Config.FuzzStrings = Config.FuzzStrings
                .Select(x => _anchors[0] + x + _anchors[1])
                .ToArray();
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            return RequestBuilder.CreateRequest(TargetFile, Server,
                Config.FuzzStrings[Mode], false, true, true);
        }

        private ScanAlert CreateAlert(FileTrace TargetTrace)
        {
            return new ScanAlert(ScanAlertOptions.Vulnerability,
                "SQL Injection", TargetTrace);
        }

        static bool IsSQLInjectable(string SQL)
        {
	        var strings = StringParser.GetStrings(SQL);
        	
	        foreach (var s in strings)
		        SQL = SQL.Replace(s, "");
        		
	        return SQL.Contains(_anchors[0]) || SQL.Contains(_anchors[1]);
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            var r = TargetTrace.Response.ToLower();
            
            foreach (var call in TargetTrace.Calls
                .Where(x => Config.Functions.Any(y => y.Name == x.Name)))
            {
                var func = Config.Functions.SingleOrDefault(x => x.Name == call.Name &&
                    x.ParamCount == call.ParameterValues.Count);

                if (func == null)
                    continue;

                var value = call.ParameterValues[func.QueryParam];

                if (IsSQLInjectable(value))
                    return CreateAlert(TargetTrace);
            }

            return null;
        }

        public override string ToString()
        {
            return "SQL Injection Scan";
        }        
    }
}
