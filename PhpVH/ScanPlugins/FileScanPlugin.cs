using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Diagnostics;
using Components.ConsolePlus;

namespace PhpVH.ScanPlugins
{
    public class FileScanPlugin : ScanPluginBase
    {
        public override int ModeCount
        {
            get { return FileScanMode.DefaultModes.Length; }
        }

        private string _server;

        public override string Server
        {
            get { return _server; }
            set { _server = value; }
        }

        FileSystemWatcher _watcher = null;

        private FileSystemEventArgs _fsArgs;

        public FileScanPlugin(string Server)
            : base()
        {
            _server = Server;            
        }

        public void StartWatching(string Path)
        {
            
        }

        void watcher_FileSystemEvent(object sender, FileSystemEventArgs e)
        {
            if (e is RenamedEventArgs)
                return;

            if (e.Name.ToLower().Contains("shell.php") ||
                e.Name.ToLower().Contains(".htaccess"))
            {
                _fsArgs = e;
            }
            else if (File.Exists(e.Name) &&
                (File.ReadAllText(e.Name).Contains("system($_GET['CMD'])") ||
                File.ReadAllText(e.Name).Contains("application/x-httpd-php .jpg")))
            {
                _fsArgs = e;
            }
        }

        protected override string BuildRequestCore(int Mode, string TargetFile, FileTrace SourceTrace)
        {
            var scanMode = FileScanMode.DefaultModes[Mode];

            string _queryString = "";

            var getFields = new List<string>();

            foreach (TracedFunctionCall c in SourceTrace.Calls.Where(x =>
                (x.Name == "$_GET" || x.Name == "$_REQUEST")))
            {
                if (c.ParameterValues.Count == 0)
                    c.ParameterValues = new List<string> { "shell_file" };

                if (getFields.Contains(c.ParameterValues[0]))
                    continue;

                getFields.Add(c.ParameterValues[0]);

                _queryString += (_queryString.Length != 0 ? "&" : "?") +
                    c.ParameterValues[0] + "=" + HttpUtility.UrlEncode(scanMode.ShellFile);
            }

            var content = "";

            var postFields = new List<string>();

            foreach (TracedFunctionCall c in SourceTrace.Calls.Where(x =>
                x.Name == "$_POST"))
            {
                if (!c.ParameterValues.Any())
                    c.ParameterValues.Add("shell_file");

                if (postFields.Contains(c.ParameterValues[0]))
                    continue;

                postFields.Add(c.ParameterValues[0]);

                content +=
                    "------x\r\n" +
                    "Content-Disposition: form-data; name=\"" + c.ParameterValues[0] + "\"\r\n" +
                    "\r\n" +
                    scanMode.ShellFile + "\r\n";
            }

            var files = SourceTrace.Calls.Where(x => x.Name == "$_FILES");

            if (files.Count() == 0)
                files = new TracedFunctionCall[]
                {
                    new TracedFunctionCall()
                    {
                        ParameterValues = new List<string>() { "shell_file" }
                    }
                };
            else
            {
                Cli.WriteLine("~Yellow~File Upload detected in {0}~R~", TargetFile);
            }

            var fileFields = new List<string>();

            foreach (TracedFunctionCall c in files)
            {
                if (c.Name == "$_FILES" &&
                    c.ParameterValues.Count == 0)
                    c.ParameterValues = new List<string>{ "shell_file" };
                if (fileFields.Contains(c.ParameterValues[0]))
                    continue;

                fileFields.Add(c.ParameterValues[0]);

                content +=
                    "------x\r\n" +
                    "Content-Disposition: form-data; name=\"" + c.ParameterValues[0] + "\"; " +
                        "filename=\"" + scanMode.ShellFile + "\"\r\n" +                    
                    "Content-Type: " + scanMode.ContentType + "\r\n" +                    
                    "\r\n" +
                    scanMode.Shell + "\r\n";                
            }

            if (content.Length > 0)
            {
                content +=
                    "------x--\r\n" +
                    "\r\n";
            }

            var header =
                "POST " + TargetFile + _queryString + " HTTP/1.1\r\n" +
                "Host: " + _server + "\r\n" +
                "Proxy-Connection: keep-alive\r\n" +
                "User-Agent: x\r\n" +
                "Content-Length: " + content.Length + "\r\n" +
                "Cache-Control: max-age=0\r\n" +
                "Origin: null\r\n" +
                "Content-Type: multipart/form-data; boundary=----x\r\n" +
                "Accept: text/html\r\n" +
                "Accept-Encoding: gzip,deflate,sdch\r\n" +
                "Accept-Language: en-US,en;q=0.8\r\n" +
                "Accept-Charset: ISO-8859-1,utf-8;q=0.7,*;q=0.3\r\n" +
                "\r\n";

            return header + content;
        }

        protected override ScanAlert ScanTraceCore(FileTrace TargetTrace)
        {
            var suspectFunctions = new string[]
            {
                "fopen",
                "file",
                "copy",
                "move_uploaded_file",
                "file_put_contents",
                "fwrite",
                "fputs"
            };

            var fileCallMatches = TargetTrace.Calls.Where(x =>
                suspectFunctions.Contains(x.Name) &&
                x.ParameterValues.Any(y => 
                    y.Contains("shell.php") ||
                    y.Contains(".htaccess")));

            if (fileCallMatches.Count() != 0)
            {
                _fsArgs = null;

                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Arbitrary File Upload", TargetTrace);                
            }
            else if (_fsArgs != null)
            {
                string eventInfo = string.Format("Type={0} Path={1}",
                    _fsArgs.ChangeType, _fsArgs.FullPath);

                if (_fsArgs is RenamedEventArgs)
                    eventInfo += " Old Path=" +
                        (_fsArgs as RenamedEventArgs).OldFullPath;

                _fsArgs = null;

                return new ScanAlert(ScanAlertOptions.Vulnerability,
                    "Arbitrary File Event - " + eventInfo, TargetTrace);
            }
            else
                return null;
        }

        public override string ToString()
        {
            return "Aribtray File Write/Change/Rename/Delete Scan";
        }

        public override void Initialize()
        {
            var path = new DirectoryInfo(Program.Config.WebRoot).Root.ToString();
            _watcher = new FileSystemWatcher(path);
            _watcher.Changed += watcher_FileSystemEvent;
            _watcher.Renamed += watcher_FileSystemEvent;
            _watcher.Created += watcher_FileSystemEvent;
            _watcher.Deleted += watcher_FileSystemEvent;
            _watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
            
            base.Initialize();
        }

        public override void Uninitialize()
        {
            _watcher.Changed -= watcher_FileSystemEvent;
            _watcher.Renamed -= watcher_FileSystemEvent;
            _watcher.Created -= watcher_FileSystemEvent;
            _watcher.Deleted -= watcher_FileSystemEvent;
            _watcher.EnableRaisingEvents = false;
            _watcher = null;
            _fsArgs = null;

            base.Uninitialize();
        }
    }    
}
