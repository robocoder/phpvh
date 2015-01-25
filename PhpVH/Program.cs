using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using PhpVH.ScanPlugins;
using System.Reflection;
using System.Security.Principal;
using System.Threading;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Components;
using PhpVH.CodeCoverage;
using Components.ConsolePlus;

namespace PhpVH
{
    class Program
    {
        public static PageFieldTable PageFieldTable = new PageFieldTable();

        public static FileIncludeTable FileIncludeTable = new FileIncludeTable();

        public static ScanConfig Config;

        public static string TraceFileName = null;

        private static bool _hook = true, _scan = true;

        private static ReportWriter _reportWriter = null;

        public static void Exit()
        {
            Console.ResetColor();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {            
#if !DEBUG
            try
            {
                Main2(args);
            }
            catch (System.Exception ex)
            {                
                ScannerCli.DisplayError(string.Format("\r\nFatal error: {0}\r\n\r\n{1}", ex.Message, ex));
            }  
#else
            Main2(args);
#endif
        }

        static void RunSelfTest()
        {
            ServiceLocator.Default.Register(Config);
            var selfTestExe = PathHelper.GetExecutingPath("PhpVH.SelfTest.exe");

            if (!File.Exists(selfTestExe))
            {
                ScannerCli.DisplayCriticalMessageAndExit("Could not find self test EXE {0}.", selfTestExe);
            }

            AppDomain.CurrentDomain.ExecuteAssembly(selfTestExe);
            //ServiceLocator.Default.Resolve<List<TestResult>>
        }

        static void Main2(string[] args)
        {
            Cli.UseTrace = true;
            Trace.AutoFlush = true;
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (!args.Any(x => x == "-l"))
            {
                ScannerCli.RunAssistant();
            }

            ScannerCli.DisplayAppInfo();

            if (args.Length == 0)
            {
                ScannerCli.DisplayInstructions();

                Exit();
            }

            Config = ScanConfig.Parse(args);

            if (Config.TestMode)
            {
                RunSelfTest();
                return;
            }

            var versionTester = new PhpVersionTester(Config);
            versionTester.CheckVersion();
            Cli.WriteLine();

            ExeProbe.Copy();

            TraceFileName = Config.WebRoot + @"\trace.txt";

            foreach (string RelativeAppPath in Config.ApplicationPaths)
            {
                string filePath = Config.WebRoot + "\\" + RelativeAppPath.Replace('/', '\\');

                if (!Directory.Exists(filePath))
                {
                    ScannerCli.DisplayCriticalMessage("Application path {0} not found.", filePath);

                    Exit();
                }

                if (Config.Repair || new DirectoryInfo(filePath)
                    .GetFiles("*.phpvhbackup", SearchOption.AllDirectories)
                    .Any(x => x.Extension.ToLower() == ".phpvhbackup"))
                {
                    ScannerCli.DisplayPhaseName("Repair");
                    Cli.WriteLine();

                    new HookCollection().Unset(new DirectoryInfo(filePath));

                    

                    if (Config.Repair)
                        continue;
                }

                _reportWriter = new ReportWriter(RelativeAppPath);

                Trace.Listeners.Clear();

                if (Config.LogConsole)
                    Trace.Listeners.Add(new TextWriterTraceListener(_reportWriter.ReportPath + "\\scan.log"));

                Trace.Listeners.Add(new ConsoleTraceListener());

                foreach (var plugin in Config.ScanPlugins)
                    plugin.Initialize();

                Program.PageFieldTable.Clear();

                Cli.WriteLine();
                ScannerCli.DisplayAppPath(RelativeAppPath);

                //////////////////////////////////////////////////////////////////////////
                // Static analysis
                ScannerCli.DisplayPhaseName("Static Analysis");

                var sae = new StaticAnalysis.StaticAnalysisEngine(Config);
                sae.FileScanned += (o, e) =>
                {
                    Cli.WriteLine(
                        "{0} [~{1}~{2}~R~]",
                        e.Item.Filename,
                        e.Item.Alerts.Any() ? ConsoleColor.Red : ConsoleColor.DarkGreen,
                        e.Item.Alerts.Length);

                    e.Item.Alerts.Iter(x => Cli.WriteLine("~Red~Potential Vulnerability: {0}~R~", x.Name));
                };
                var staticAnalysisAlerts = sae.ScanDirectory(filePath);

                if (staticAnalysisAlerts.Any())
                    _reportWriter.Write("Static analysis", staticAnalysisAlerts.ToXml(), "xml");

                Cli.WriteLine();
                Cli.WriteLine();

                // End Static analysis
                //////////////////////////////////////////////////////////////////////////

                if (!Config.StaticOnly)
                {
                    ScanMetrics.Default = new ScanMetrics();
                    ScanMetrics.Default.Annotator.AnnotationFile = new FileInfo(Config.WebRoot + "\\Annotation.txt");

                    #region Hooks
                    var hooks2 = new HookCollection(Hook.GetDefaults());
                    var sqlPlugin = new SqlScanPlugin(null);
                    sqlPlugin.Initialize();
                    hooks2.AddRange(sqlPlugin.Config.Functions.ToHooks());
                    #endregion

                    if (Config.HookSuperglobals)
                    {
                        hooks2.AddRange(Hook.GetSuperglobals());
                    }

                    if (_scan)
                    {
                        ScannerCli.DisplayPhaseName("Form scrape");

                        var urlDictionary = CreateUrlDictionary(RelativeAppPath, new DirectoryInfo(filePath));

                        foreach (var page in urlDictionary.Select(x => new
                        {
                            Relative = x.Key,
                            Url = "http://" + Config.Server + x.Key,
                        }))
                        {
                            var data = WebClientHelper.DownloadData(page.Url);
                            var respStr = ASCIIEncoding.ASCII.GetString(data.Data);
                            var forms = FormScraper.GetForms(respStr, page.Url);

                            if (forms.Any())
                            {
                                foreach (var f in forms)
                                {
                                    var action = new Uri(f.Action);
                                    if (!action.Host.Contains(Config.Server) ||
                                        !urlDictionary.ContainsKey(action.LocalPath))
                                        continue;

                                    var file = urlDictionary[action.LocalPath];

                                    if (!PageFieldTable.ContainsKey(file))
                                        PageFieldTable.Add(file, new Dictionary<string, List<string>>());

                                    var superglobal = "$_" + f.Method.ToUpper();

                                    if (!PageFieldTable[file].ContainsKey(superglobal))
                                        PageFieldTable[file].Add(superglobal, new List<string>());

                                    var newInputs = f.Inputs
                                        .Select(x => x.Name ?? x.Type)
                                        .Where(x => x != null && !PageFieldTable[file][superglobal].Contains(x));

                                    PageFieldTable[file][superglobal].AddRange(newInputs);
                                }
                            }

                            ScannerCli.DisplayScrapedUrl(page.Relative, forms);
                        }

                        Trace.WriteLine("");

                        if (_hook)
                        {
                            ScannerCli.DisplayPhaseName("Dynamic Analysis Initialization");
                            Cli.WriteLine();

                            hooks2.Set(new DirectoryInfo(filePath));
                            hooks2.CreateHandlerFile();
                            Cli.WriteLine();
                            Cli.WriteLine();

                            Program.Config.ScanPlugins
                                .Iter(x =>
                                {
                                    var annotationTableClone = ScanMetrics.Default.Annotator.AnnotationTable.Clone() as AnnotationTable;
                                    annotationTableClone.Plugin = x.ToString();
                                    ScanMetrics.Default.PluginAnnotations.Add(annotationTableClone);
                                });
                        }

                        ScannerCli.DisplayPhaseName("Dynamic Analysis");

                        ScanDirectory(new DirectoryInfo(filePath), RelativeAppPath);
                        Console.WriteLine();
                    }

                    ScannerCli.DisplayPhaseName("Dynamic Analysis Uninitialization");
                    Cli.WriteLine();
                    hooks2.DeleteHandlerFile();

                    File.Delete(TraceFileName);

                    foreach (var plugin in Config.ScanPlugins)
                        plugin.Uninitialize();

                    if (Config.Unhook)
                    {
                        hooks2.Unset(new DirectoryInfo(filePath));

                        Cli.WriteLine();
                        Cli.WriteLine();
                    }
                }

                var reportFiles = _reportWriter.WriteFilenames();
#if !MONO && !NET35
                if (_reportWriter.ReportFiles.Any() && Config.RunViewer)
                {
                    var viewerPath = Assembly.GetExecutingAssembly().Location.RemoveAtLastIndexOf('\\', 1) +
                        @"PHPVHReportViewer.exe";

                    if (File.Exists(viewerPath))
                        System.Diagnostics.Process.Start(viewerPath, "\"" + reportFiles + "\"");
                    else
                        System.Windows.Forms.MessageBox.Show("Could not locate report viewer executable.",
                            "Error launching report viewer", System.Windows.Forms.MessageBoxButtons.OK,
                            System.Windows.Forms.MessageBoxIcon.Error);

                }
#endif
            }

            Trace.Listeners.Clear();
            Trace.Listeners.Add(new ConsoleTraceListener());            
        }

        static string CreateUrl(string RelativeAppPath, DirectoryInfo AppDirectory,
            FileInfo AppFile)
        {
            return System.Web.HttpUtility.UrlPathEncode("/" + RelativeAppPath + "/" +
                    AppFile.FullName.Remove(0, AppDirectory.FullName.Length + 1).Replace('\\', '/'));
        }

        static Dictionary<string, string> CreateUrlDictionary(string RelativeAppPath, DirectoryInfo AppDirectory)
        {
            return AppDirectory
                .GetFiles("*.php", SearchOption.AllDirectories)
                .Where(x => x.Extension.ToLower() == ".php")
                .ToDictionary(x => CreateUrl(RelativeAppPath, AppDirectory, x), x => x.FullName);
        }        

        static void ScanDirectory(DirectoryInfo AppDirectory, string RelativeAppPath)
        {
            var ignoreExtensions = new[]
            {
                "png",
                "jpg",
                "jpeg",
                "gif",
                "ico",
                "js",
                "css",
            };

            var urlDictionary = CreateUrlDictionary(RelativeAppPath, AppDirectory);

            var urlCollections = new[]
            {
                urlDictionary.Select(x => x.Key).ToList(),
                new List<string>()
            };

            var traceTable = new TraceTable();

            var report = new StringBuilder();

            var coverageReport = new StringBuilder();

            var map = new ApplicationMap();

            var alerts = new ScanAlertCollection();

            int requestCount = 0;

            MessageDumper messageDumper = null;

            if (Config.DumpMessages)
            {
                messageDumper = new MessageDumper(_reportWriter.ReportPath.FullName);
            }

            for (int urlCollectionIndex = 0; urlCollectionIndex < 2; urlCollectionIndex++)
                foreach (ScanPluginBase plugin in Config.ScanPlugins)
                {
                    if (Config.CodeCoverageReport > 0)
                    {
                        ScanMetrics.Default.Annotator.Reset();
                        ScanMetrics.Default.Annotator.AnnotationTable = ScanMetrics.Default.PluginAnnotations[plugin.ToString()];
                    }

                    if (!urlCollections[urlCollectionIndex].Any())
                        continue;

                    ScannerCli.DisplayScanPlugin(plugin);

                    foreach (var remotePath in urlCollections[urlCollectionIndex])
                    {
                        ScannerCli.DisplayResourcePath(remotePath);

                        IEnumerable<IEnumerable<TracedFunctionCall>> calls = null;
                        if (urlCollectionIndex == 0)
                        {
                            var key = urlDictionary[remotePath];

                            if (Program.PageFieldTable.ContainsKey(key))
                                calls = Program.PageFieldTable[key]
                                    .Select(x => x.Value.Select(y => new TracedFunctionCall()
                                    {
                                        Name = x.Key,
                                        ParameterValues = new List<string>() { y }
                                    }));
                        }

                        for (int i = 0; i < plugin.ModeCount; i++)
                        {
                            foreach (var useStaticAnalysisInputs in new[]
                        {
                            false,
                            true
                        })
                            {
                                var trace = new FileTrace();

                                if (useStaticAnalysisInputs && calls != null)
                                {
                                    if (!calls.Any(x => x.Any()))
                                        continue;

                                    foreach (var c in calls)
                                        trace.Calls.AddRange(c);
                                }

                                bool discoveredVars = true;

                                while (discoveredVars)
                                {
                                    var traceFile = new FileInfo(TraceFileName);

                                    IOHelper.TryAction(traceFile.Delete);

                                    var client = new TcpClient()
                                    {
                                        ReceiveTimeout = Config.Timeout,
                                        SendTimeout = Config.Timeout
                                    };

                                    while (!client.Connected)
                                        try
                                        {
                                            client.Connect(Config.Server, Config.Port);
                                        }
                                        catch (SocketException ex)
                                        {
                                            ScannerCli.DisplayError(ex.Message);

                                            Thread.Sleep(5000);
                                        }

                                    client.LingerState = new LingerOption(true, 0);

                                    HttpResponse resp = null;

                                    string req, respString = "";

                                    using (var stream = client.GetStream())
                                    {
                                        req = plugin.BuildRequest(i, remotePath, trace);

                                        if (Config.DumpMessages)
                                            messageDumper.Dump(req, requestCount, MessageType.Request);

                                        var stopwatch = new Stopwatch();
                                        stopwatch.Start();

                                        stream.WriteString(req);

                                        try
                                        {
                                            var sgs = trace.Calls
                                                .Superglobals()
                                                .Select(x => new { x.Name, Value = x.ParameterValues.Count > 0 ? x.ParameterValues[0] : null })
                                                .Distinct();

                                            var discoveredVarCount = sgs.Count();

                                            var reader = new HttpResponseReader(stream);
                                            resp = reader.Read();
                                            stopwatch.Stop();

                                            respString = resp.CompleteResponse;

                                            ScannerCli.DisplayResponse(
                                                resp, 
                                                i, 
                                                discoveredVarCount, 
                                                stopwatch.ElapsedMilliseconds,
                                                resp.CompleteResponse.Length);
                                        }
                                        catch (SocketException)
                                        {
                                            ScannerCli.DisplayResponseError(respString);
                                        }
                                        catch (IOException)
                                        {
                                            ScannerCli.DisplayResponseError(respString);
                                        }
                                    }

                                    if (urlCollectionIndex == 0 && resp != null)
                                    {
                                        var abs = "http://" + Config.Server + remotePath;

                                        var urls = new UriScraper()
                                        {
                                            Regex = new Regex(@"[/.]" + Config.Server + @"($|/)", RegexOptions.IgnoreCase | RegexOptions.Compiled),
                                        }
                                        .Parse(resp.Body, abs)
                                        .Select(x => new Uri(x).LocalPath);

                                        foreach (var url in urls)
                                        {
                                            if (!ignoreExtensions.Any(x => url.ToLower().EndsWith("." + x)) &&
                                                !urlCollections[0].Contains(url) &&
                                                !urlCollections[1].Contains(url))
                                            {
                                                urlCollections[1].Add(url);

                                                ScannerCli.DisplayDiscoveredUrl(url);
                                            }
                                        }
                                    }

                                    client.Close();

                                    if (Config.DumpMessages)
                                        messageDumper.Dump(respString, requestCount, MessageType.Response);

                                    requestCount++;

                                    traceFile = new FileInfo(TraceFileName);

                                    FileTrace newTrace = null;

                                    IOHelper.TryAction(() =>
                                    {
                                        if (traceFile.Exists)
                                            using (var reader = traceFile.OpenText())
                                                newTrace = FileTrace.Parse(reader);
                                    });

                                    if (newTrace == null)
                                        newTrace = new FileTrace();
                                    newTrace.Request = req;
                                    newTrace.Response = respString;
                                    newTrace.File = remotePath;

                                    var alert = plugin.ScanTrace(newTrace);
                                    
                                    if (alert != null)
                                    {
                                        if (Config.BeepOnAlert)
                                            Console.Beep();

                                        ScannerCli.DisplayAlert(alert);

                                        alerts.Add(alert);
                                        report.Append(alert.ToString());
                                    }

                                    discoveredVars = false;

                                    foreach (TracedFunctionCall c in newTrace.Calls
                                        .Superglobals()
                                        .Where(x => x.ParameterValues.Any()))
                                    {
                                        var oldCalls = trace.Calls.Where(x =>
                                            x.Name == c.Name &&
                                            x.ParameterValues.SequenceEqual(c.ParameterValues));
                                        if (!oldCalls.Any())
                                        {
                                            discoveredVars = true;
                                            break;
                                        }
                                    }

                                    var orphanedInputs = trace.Calls
                                        .Superglobals()
                                        .Where(x => !newTrace.Calls
                                            .Any(y => x.Name == y.Name && x.ParameterValues
                                                .SequenceEqual(y.ParameterValues)))
                                        .ToArray();

                                    newTrace.Calls.AddRange(orphanedInputs);

                                    trace = newTrace;
                                }

                                var superglobals = trace.Calls.Superglobals();

                                map.AddTrace(trace);

                                if (Config.DiscoveryReport)
                                {
                                    if (!traceTable.ContainsKey(plugin))
                                        traceTable.Add(plugin, new Dictionary<int, Dictionary<string, FileTrace>>());

                                    if (!traceTable[plugin].ContainsKey(i))
                                        traceTable[plugin].Add(i, new Dictionary<string, FileTrace>());

                                    if (!traceTable[plugin][i].ContainsKey(trace.File))
                                        traceTable[plugin][i].Add(trace.File, trace);
                                    else
                                        traceTable[plugin][i][trace.File] = trace;
                                }
                            }
                        }
                    }

                    if (Config.CodeCoverageReport > 0 && urlCollectionIndex == 0)
                    {
                        Cli.WriteLine("Calculating code coverage...");

                        CodeCoverageTable coverage = null;

                        IOHelper.TryAction(() =>
                        {
                            var calculator = new CodeCoverageCalculator(
                                ScanMetrics.Default.Annotator.AnnotationFile,
                                ScanMetrics.Default.PluginAnnotations[plugin.ToString()]);
                            coverage = calculator.CalculateCoverage();
                        });

                        coverage.Plugin = plugin.ToString();

                        coverageReport.AppendLine(coverage.ToString() + "\r\n");
                    }

                    Cli.WriteLine();
                }

            _reportWriter.Write("Vulnerability Report", report.ToString());

            _reportWriter.Write("Input Map Report", map.ToXml(), "xml");


            if (alerts.Any())
                _reportWriter.Write("Vulnerability Report",
                    alerts.ToXml(), "pxml");

            if (Config.DiscoveryReport)
                _reportWriter.Write("Scan Overview Report",
                    DiscoveryReport.Create(traceTable));

            if (Config.CodeCoverageReport > 0)
            {
                _reportWriter.Write("Code Coverage Report", coverageReport.ToString());

                var annotationXml = ScanMetrics.Default.PluginAnnotations.ToXml();
                var annotationFile = _reportWriter.Write(
                    "Annotation",
                    annotationXml,
                    "axml");

                Cli.WriteLine();
                var commenter = new CoverageCommenter(ScanMetrics.Default.PluginAnnotations);
                commenter.LoadTable(annotationFile);                
                commenter.WriteCommentedFiles(_reportWriter.ReportPath.FullName);
                _reportWriter.ReportFiles.Add(
                    new ReportFile(
                        "Coverage Comments",
                        Path.Combine("Code Coverage", "index.html")));
            }
        }
    }
}
