using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Components.Aphid.Interpreter;
using Components.ConsolePlus;

namespace PhpVH.ScanPlugins
{
    public class CommandScanPlugin : ScriptableScanPluginBase<CommandScanConfig>
    {
        private Thread _processThread;

        private bool _probeDetected = false;

        public override int ModeCount
        {
            get { return Config.TestCases.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        public CommandScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        protected override void InitializeCore()
        {
            if (!WasScriptCached)
            {
                Cli.WriteLine(
                    "~Green~{0}~R~ test cases and ~Green~{1}~R~ function names loaded",
                    Config.TestCases.Length,
                    Config.Functions.Length);
            }
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            _processThread = new Thread(x =>
            {
                while (true)
                {
                    Thread.Sleep(500);

                    var probes = Process.GetProcessesByName("PHPVHProbe");

                    if (probes.Any())
                    {
                        _probeDetected = true;
                        foreach (var p in probes)
                            try
                            {
                                p.Kill();
                            }
                            catch { }
                    }
                }
            }) { IsBackground = true };
            _processThread.Start();

            return RequestBuilder.CreateRequest(TargetFile, Server,
                Config.TestCases[Mode], false, false, true);
        }        

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            try
            {
                _processThread.Abort();
            }
            catch { }

            var detected = _probeDetected;

            if (detected)
                Trace.WriteLine("Probe detected");

            _probeDetected = false;

            if (TargetTrace.Calls
                .Where(x => Config.Functions.Contains(x.Name))
                .Any(x => 
                    x.ParameterValues.Any() && 
                    x.ParameterValues
                        .Select(y => y.ToLower())
                        .Any(y => y.Contains(Config.ProbeName.ToLower()))))
            {
                foreach (var c in Process.GetProcessesByName(Config.ProbeName))
                {
                    c.Kill();
                    c.WaitForExit();
                }

                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Command Execution", TargetTrace);
            }

            var processes = Process.GetProcessesByName(Config.ProbeName);

            if (detected || processes.Length != 0)
            {
                foreach (Process p in processes)
                {
                    p.Kill();
                    p.WaitForExit();
                }

                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Command Execution", TargetTrace);
            }
            else
                return null;
        }

        public override string ToString()
        {
            return "Command Execution Scan";
        }        
    }
}
