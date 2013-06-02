using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PhpVH;
using PhpVH.CodeCoverage;
using Components;

namespace PhpVH.Tests.Integration
{
    public sealed class PhpVHTester : IDisposable
    {
        private const string _noPxml = "No PXML file found. This generally means the test failed to find any vulnerabilities.";

        public string Output { get; private set; }

        public string Name { get; set; }

        public string Mode { get; set; }

        public int CodeCoverage { get; set; }

        public bool HookSuperglobals { get; set; }

        public string[] Php { get; set; }

        public DirectoryInfo TestDirectory { get; set; }

        private DirectoryInfo _reportDir = new DirectoryInfo("Reports");

        public TestSettings Settings { get; set; }

        public PhpVHTester()
        {
            ResolveSettings();
        }

        public PhpVHTester(string name, string mode, string[] php)
            : this()
        {
            Name = name;
            Mode = mode;
            Php = php;
        }

        private void ResolveSettings()
        {
            Settings = ServiceLocator.Default.ResolveOrCreate(() =>
            {
                var settings = new TestSettings();
                settings.LoadAppSettings();
                return settings;
            });
        }

        public IEnumerable<DirectoryInfo> GetTestReports()
        {
            _reportDir.Refresh();
            return _reportDir.Exists ?
                _reportDir.GetDirectories().Where(x => x.Name.StartsWith(Name)) :
                new DirectoryInfo[0];
        }

        private string CreateArgs()
        {
            var coverageArg =
                CodeCoverage == 2 ? "-c2" :
                CodeCoverage == 1 ? "-c" :
                "";

            return string.Format(
                "-s {0} {1} -l -m {2} {3} {4}Test {5} -p {6}",
                Settings.Host,
                coverageArg,
                Mode,
                Settings.Webroot,
                Name,
                HookSuperglobals ? "-h" : "",
                Settings.Port);
        }

        private string[] CreateArgs2()
        {
            var coverageArg =
                CodeCoverage == 2 ? "-c2" :
                CodeCoverage == 1 ? "-c" :
                "";

            var args = new List<string> 
            { 
                "-s", 
                Settings.Host, 
                "-p",
                Settings.Port.ToString(),
                "-l", 
                "-m", 
                Mode, 
                Settings.Webroot, 
                Name + "Test" 
            };

            if (coverageArg != "")
            {
                args.Insert(0, coverageArg);
            }

            if (HookSuperglobals)
            {
                args.Insert(0, "-h");
            }

            return args.ToArray();
        }

        private void LaunchProcess()
        {
            var args = CreateArgs2()
                    .Select(x => x.Any(Char.IsWhiteSpace) ?
                        "\"" + x + "\"" :
                        x)
                    .Aggregate((x, y) => x + " " + y);

            var sb = new StringBuilder();

            var process = new Process();
            process.StartInfo.FileName = "phpvh.exe";
            process.StartInfo.Arguments = args;

            if (Settings.LogConsoleOutput)
            {
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;

                DataReceivedEventHandler handler = (s, e) =>
                {
                    sb.AppendLine(e.Data);
                };

                process.OutputDataReceived += handler;
                process.ErrorDataReceived += handler;
            }

            process.Start();

            if (Settings.LogConsoleOutput)
            {
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
            }

            process.WaitForExit();
            Output = sb.ToString();
        }

        public void DumpOutput()
        {
            var dumpName = string.Format("Output-{0}.txt", Guid.NewGuid());

            File.WriteAllText(dumpName, Output);
        }

        public void RunPhpVH()
        {
            if (!Directory.Exists(Settings.Webroot))
            {
                throw new DirectoryNotFoundException(string.Format("Directory {0} not found.", Settings.Webroot));
            }

            TestDirectory = new DirectoryInfo(Path.Combine(Settings.Webroot, Name + "Test"));

            if (TestDirectory.Exists)
            {
                TestDirectory.Delete(true);
            }

            TestDirectory.Create();
            
            GetTestReports().Iter(x => x.Delete(true));

            Php
                .Concat(new[] { "Php\\FalsePositiveCheck.php" })
                .Iter(x => File.Copy(x, Path.Combine(TestDirectory.FullName, Path.GetFileName(x))));

            if (Settings.LaunchInAppDomain)
            {
                var domain = AppDomain.CreateDomain("testDomain");
                domain.ExecuteAssembly("phpvh.exe", CreateArgs2());
                AppDomain.Unload(domain);
            }
            else
            {
                LaunchProcess();
            }
        }

        public ScanAlertCollection LoadAlerts()
        {
            var reports = GetTestReports();

            Assert.AreNotEqual(
                0, 
                reports.Count(),
                "No vulnerability reports found for {0}",
                Name);

            Assert.AreEqual(
                1,
                reports.Count(),
                "More than one report for {0} found",
                Name);

            var report = reports.First();
            var reportFiles = report.GetFiles();
            var pxmlFile = reportFiles.FirstOrDefault(x => x.Extension == ".pxml");
            Assert.IsNotNull(pxmlFile, _noPxml);
            return ScanAlertCollection.Load(pxmlFile.FullName);

        }

        public CodeCoverageTable LoadCoverage()
        {
            var report =
                GetTestReports()
                .First()
                .GetFiles()
                .FirstOrDefault(x => x.FullName.ToLower().EndsWith("annotation.axml"));

            var table = PluginAnnotationTable.Load(report.FullName);
            var annotationTable = table.Items.First();

            var calculator = new CodeCoverageCalculator(annotationTable);
            return calculator.CalculateCoverage(false);
        }

        public void Dispose()
        {
            if (TestDirectory == null)
                return;

            TestDirectory.Refresh();

            if (TestDirectory.Exists)
            {
                TestDirectory.Delete(true);
            }
        }
    }
}
