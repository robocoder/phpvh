using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Components;
using Components.Aphid.Interpreter;
using Components.ConsolePlus;

namespace PhpVH.ScanPlugins
{
    public class LocalFileInclusionScanPlugin : ScriptableScanPluginBase
    {
        public const string ProbeFile = "lfi_test.txt";

        private string[] _traversalSequences;

        private string[] _suspectFunctions = new string[]
        {
            "include",
            "require",
            "require_once"
        };

        private string _badChars = "LFI_Test123";

        public override int ModeCount
        {
            get { return _traversalSequences.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        FileSystemWatcher watcher;

        public LocalFileInclusionScanPlugin(string Server)
            : base()
        {
            _server = Server;
        }

        private FileSystemEventArgs _fsArgs;

        void watcher_FileSystemEvent(object sender, FileSystemEventArgs e)
        {
            if (e.Name == ProbeFile)
            {
                _fsArgs = e;

                if (e.ChangeType == WatcherChangeTypes.Deleted)
                    WriteProbe();
            }
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            return RequestBuilder.CreateRequest(TargetFile, Server, _traversalSequences[Mode], 
                false, false, true);
        }

        private void WriteProbe()
        {
            using (var stream = System.IO.File.OpenWrite(@"C:\" + ProbeFile))
                stream.WriteString(_badChars + "<?php echo 5200 + 30; ?>");
        }

        public void DeleteProbe()
        {
            File.Delete(@"C:\" + ProbeFile);
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            if (_fsArgs != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;                

                watcher.EnableRaisingEvents = false;

                WriteProbe();

                watcher.EnableRaisingEvents = true;

                var c = _fsArgs.ChangeType;
                
                _fsArgs = null;

                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "File System Event: " + c, TargetTrace);
            }

            var badCharsLowered = _badChars.ToLower();

            if (TargetTrace.Response.ToLower().Contains(badCharsLowered))
            {
                return new ScanAlert(ScanAlertOptions.Vulnerability,
                        TargetTrace.Response.ToLower().Contains(badCharsLowered + "5230") ?
                            "Local File Inclusion" :
                            "Arbitrary File Read",
                        TargetTrace);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return "Local File Inclusion/Arbitrary Read Scan";
        }

        public override void Uninitialize()
        {
            watcher.Changed -= watcher_FileSystemEvent;
            watcher.Created -= watcher_FileSystemEvent;
            watcher.Deleted -= watcher_FileSystemEvent;
            watcher.EnableRaisingEvents = true;
            watcher = null;
            _fsArgs = null;

            DeleteProbe();

            base.Uninitialize();
        }

        protected override void InitializeCore()
        {
            _traversalSequences = ScriptLauncher.LoadTestCases(ScriptResult);

            try
            {
                WriteProbe();
            }
            catch (System.Exception)
            {
                ScannerCli.DisplayCriticalMessageAndExit("Could not create LFI probe. Please run PHPVH as administrator.");
            }

            var path = new DirectoryInfo(Program.Config.WebRoot).Root.ToString();
            watcher = new FileSystemWatcher(path);
            watcher.Changed += watcher_FileSystemEvent;
            watcher.Created += watcher_FileSystemEvent;
            watcher.Deleted += watcher_FileSystemEvent;
            watcher.EnableRaisingEvents = true;
        }
    }    
}
