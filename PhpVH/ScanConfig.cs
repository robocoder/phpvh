using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using Components;
using PhpVH.ScanPlugins;

namespace PhpVH
{
    [Serializable]
    [XmlRoot("Scan")]
    public class ScanConfig : INotifyPropertyChanged, ICloneable
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private static XmlSerializer _serializer = new XmlSerializer(typeof(ScanConfig));

        private bool _launcherUsed = false;

        [XmlAttribute]
        public bool LauncherUsed
        {
            get { return _launcherUsed; }
            set { _launcherUsed = value; }
        }

        private bool _logConsole = false;

        [XmlAttribute]
        public bool LogConsole
        {
            get { return _logConsole; }
            set { _logConsole = value; }
        }

        private bool _testMode = false;

        [XmlIgnore]
        public bool TestMode
        {
            get { return _testMode; }
            set { _testMode = value; }
        }

        private bool _BeepOnAlert = false;
        
        [XmlAttribute]
        public bool BeepOnAlert
        {
        	get { return _BeepOnAlert; }
        	set
        	{                
        		_BeepOnAlert = value;
        
        		RaisePropertyChanged("BeepOnAlert");
        	}
        }

        private bool _unhook = true;

        [XmlAttribute]
        public bool Unhook
        {
            get { return _unhook; }
            set { _unhook = value; RaisePropertyChanged("Unhook"); }
        }

        private bool _runViewer = false;

        [XmlAttribute]
        public bool RunViewer
        {
            get { return _runViewer; }
            set { _runViewer = value; RaisePropertyChanged("RunViewer"); }
        }

        private bool _DumpMessages;
        
        [XmlAttribute]
        public bool DumpMessages
        {
        	get { return _DumpMessages; }
        	set
        	{                
        		_DumpMessages = value;
        
        		RaisePropertyChanged("DumpMessages");
        	}
        }

        private bool _arbitraryPHPScan = true;

        [XmlAttribute("ArbitraryPHP")]
        public bool ArbitraryPHPScan
        {
            get { return _arbitraryPHPScan; }
            set { _arbitraryPHPScan = value; RaisePropertyChanged("ArbitraryPHPScan"); }
        }

        private bool _commandScan = true;

        [XmlAttribute("Command")]
        public bool CommandScan
        {
            get { return _commandScan; }
            set { _commandScan = value; RaisePropertyChanged("CommandScan"); }
        }

        private bool _fileScan = true;

        [XmlAttribute("File")]
        public bool FileScan
        {
            get { return _fileScan; }
            set { _fileScan = value; RaisePropertyChanged("FileScan"); }
        }

        private bool _lfiScan = true;

        [XmlAttribute("LFI")]
        public bool LFIScan
        {
            get { return _lfiScan; }
            set { _lfiScan = value; RaisePropertyChanged("LFIScan"); }
        }

        private bool _sqlScan = true;

        [XmlAttribute("SQL")]
        public bool SQLScan
        {
            get { return _sqlScan; }
            set { _sqlScan = value; RaisePropertyChanged("SQLScan"); }
        }

        private bool _dynamicScan = true;

        [XmlAttribute("Dynamic")]
        public bool DynamicScan
        {
            get { return _dynamicScan; }
            set { _dynamicScan = value; RaisePropertyChanged("DynamicScan"); }
        }

        private bool _hookSuperglobals = true;

        [XmlAttribute("HookSuperglobals")]
        public bool HookSuperglobals
        {
            get { return _hookSuperglobals; }
            set { _hookSuperglobals = value; RaisePropertyChanged("HookSuperglobals"); }
        }

        private bool _xssScan = true;

        [XmlAttribute("XSS")]
        public bool XSSScan
        {
            get { return _xssScan; }
            set { _xssScan = value; RaisePropertyChanged("XSSScan"); }
        }

        private bool _openRedirect = true;

        [XmlAttribute]
        public bool OpenRedirect
        {
            get { return _openRedirect; }
            set { _openRedirect = value; RaisePropertyChanged("OpenRedirect"); }
        }

        private bool _pathDisclosure = true;

        [XmlAttribute]
        public bool PathDisclosure
        {
            get { return _pathDisclosure; }
            set { _pathDisclosure = value; RaisePropertyChanged("PathDisclosure"); }
        }

        private string _server = "localhost";

        [XmlAttribute]
        public string Server
        {
            get { return _server; }
            set { _server = value; RaisePropertyChanged("Server"); }
        }

        private int _port = 80;

        [XmlAttribute]
        public int Port
        {
            get { return _port; }
            set { _port = value; RaisePropertyChanged("Port"); }
        }

        private string _webRoot;

        [XmlAttribute]
        public string WebRoot
        {
            get { return _webRoot; }
            set { _webRoot = value; RaisePropertyChanged("WebRoot"); }
        }

        private void CodeCoverageChanged()
        {
            RaisePropertyChanged("CodeCoverageReport",
                "CodeCoverageNone", "CodeCoverageNormal",
                "CodeCoverageHigh");
        }

        private int _codeCoverageReport;

        [XmlAttribute]
        public int CodeCoverageReport
        {
            get { return _codeCoverageReport; }
            set 
            { 
                _codeCoverageReport = value;                 

                CodeCoverageChanged(); 
            }
        }

        [XmlIgnore]
        public bool CodeCoverageNone
        {
            get { return _codeCoverageReport == 0; }
            set { 
                _codeCoverageReport = 0; CodeCoverageChanged(); }
        }

        [XmlIgnore]
        public bool CodeCoverageNormal
        {
            get { return _codeCoverageReport == 1; }
            set { 
                _codeCoverageReport = 1; CodeCoverageChanged(); }
        }

        [XmlIgnore]
        public bool CodeCoverageHigh
        {
            get { return _codeCoverageReport == 2; }
            set { 
                _codeCoverageReport = 2; CodeCoverageChanged(); }
        }

        private bool _discoveryReport;

        [XmlAttribute]
        public bool DiscoveryReport
        {
            get { return _discoveryReport; }
            set { _discoveryReport = value; RaisePropertyChanged("DiscoveryReport"); }
        }

        private string[] _applicationPaths;

        [XmlElement("ApplicationPath")]
        public string[] ApplicationPaths
        {
            get { return _applicationPaths; }
            set { _applicationPaths = value; RaisePropertyChanged("ApplicationPaths"); }
        }

        private string _appPathsString;

        [XmlAttribute]
        public string AppPathsString
        {
            get { return _appPathsString; }
            set { _appPathsString = value; RaisePropertyChanged("AppPathsString"); }
        }

        private bool _allApps = true;

        [XmlAttribute]
        public bool AllApps
        {
            get { return _allApps; }
            set { _allApps = value; RaisePropertyChanged("AllApps"); }
        }

        private bool _repair = false;

        [XmlAttribute]
        public bool Repair
        {
            get { return _repair; }
            set { _repair = value; }
        }

        private bool _staticOnly;

        [XmlAttribute]
        public bool StaticOnly
        {
            get { return _staticOnly; }
            set { _staticOnly = value; }
        }

        private List<ScanPluginBase> _ScanPlugins = new List<ScanPluginBase>();

        [XmlIgnore]
        public List<ScanPluginBase> ScanPlugins
        {
            get { return _ScanPlugins; }
        }

        private int _timeout = 60000;

        [XmlAttribute]
        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; RaisePropertyChanged("Timeout"); }
        }

        public virtual void Serialize(string File)
        {
            _serializer.Serialize(File, this);
        }

        private void RaisePropertyChanged(params string[] Properties)
        {
            if (PropertyChanged != null)
                foreach (var p in Properties)
                    PropertyChanged(this, 
                        new PropertyChangedEventArgs(p));
        }

        public List<string> Validate()
        {
            var errors = new List<string>();

            if (_webRoot == null)
                errors.Add("No webroot specified");
            else if (!new DirectoryInfo(_webRoot).Exists)
                errors.Add(string.Format("Could not find webroot {0}",
                    _webRoot));
            else if (!AllApps)
            {
                if (string.IsNullOrEmpty(_appPathsString))
                    errors.Add("No applications paths specified. Use * to target every " +
                        "application in the webroot");
                else
                {
                    foreach (var appPath in _appPathsString
                        .Split(',')
                        .Select(x => _webRoot + "\\" + x.Trim('"')))
                        if (!new DirectoryInfo(appPath).Exists)
                            errors.Add(string.Format("Could not find application path {0}",
                                appPath));
                }
            }

            return errors;
        }

        public string GetArgs()
        {
            if (Repair)
            {
                return "-r \"" + WebRoot + "\" " + (AllApps ? "*" : AppPathsString);
            }

            var args = string.Format("-s {0} ", Server);

            args += "-m ";

            if (CommandScan)
                args += "C";

            if (FileScan)
                args += "F";

            if (LFIScan)
                args += "L";

            if (ArbitraryPHPScan)
                args += "P";

            if (SQLScan)
                args += "S";

            if (DynamicScan)
                args += "D";

            if (XSSScan)
                args += "X";

            if (OpenRedirect)
                args += "R";

            if (PathDisclosure)
                args += "I";

            args += " ";

            if (DiscoveryReport)
                args += "-d ";

            if (BeepOnAlert)
                args += "-b ";

            //if (HookSuperglobals)
            //    args += "-h ";

            if (DumpMessages)
                args += "-dump ";

            if (RunViewer)
                args += "-v ";

            if (StaticOnly)
                args += "-static ";

            if (CodeCoverageNormal)
                args += "-c ";
            else if (CodeCoverageHigh)
                args += "-c2 ";

            if (LogConsole)
                args += "-log ";

            args += "-t " + Timeout.ToString() + " ";

            args += "-p " + Port.ToString() + " ";

            args += "\"" + WebRoot + "\" " + (AllApps ? "*" : AppPathsString);

            return args;
        }

        public static ScanConfig Deserialize(string File)
        {
            var config = _serializer.Deserialize(File) as ScanConfig;
            config.HookSuperglobals = true;
            return config;
        }

        public static ScanConfig Parse(string[] args)
        {
            var config = new ScanConfig();

            int argIndex = 0;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-s")
                {
                    if (args.Length == i + 1)
                    {
                        ScannerCli.DisplayCriticalMessageAndExit("-s argument expects value");
                    }

                    config.Server = args[i + 1];
                    i++;

                    foreach (var a in config.ScanPlugins)
                    {
                        a.Server = config.Server;
                    }
                }
                else if (args[i] == "-static")
                {
                    config.StaticOnly = true;
                }
                else if (args[i] == "-t")
                {
                    int timeout = 0;
                    if (args.Length == i + 1 ||
                        !int.TryParse(args[i + 1], out timeout))
                    {
                        ScannerCli.DisplayCriticalMessageAndExit("-t argument expects number value");
                    }

                    i++;
                    config.Timeout = timeout;
                }
                else if (args[i] == "-p")
                {
                    int port = 0;
                    if (args.Length == i + 1 ||
                        !int.TryParse(args[i + 1], out port))
                    {
                        ScannerCli.DisplayCriticalMessageAndExit("-p argument expects number value");
                    }

                    i++;
                    config.Port = port;
                }
                else if (args[i] == "-l")
                {
                    config.LauncherUsed = true;
                }
                else if (args[i] == "-n")
                {
                    config.Unhook = false;
                }
                else if (args[i] == "-v")
                {
                    config.RunViewer = true;
                }
                else if (args[i] == "-d")
                {
                    config.DiscoveryReport = true;
                }
                else if (args[i] == "-c")
                {
                    config.CodeCoverageReport = 1;
                }
                else if (args[i] == "-c2")
                {
                    config.CodeCoverageReport = 2;
                }
                else if (args[i] == "-dump")
                {
                    config.DumpMessages = true;
                }
                else if (args[i] == "-b")
                {
                    config.BeepOnAlert = true;
                }
                else if (args[i] == "-log")
                {
                    config.LogConsole = true;
                }
                else if (args[i] == "-test")
                {
                    config.TestMode = true;
                }
                else if (args[i] == "-r")
                {
                    config.Repair = true;
                }
                //else if (args[i] == "-h")
                //    config.HookSuperglobals = true;
                else if (args[i] == "-l")
                {
                    // Nothing
                }
                else if (args[i] == "-m")
                {
                    if (args.Length == i + 1)
                    {
                        ScannerCli.DisplayCriticalMessageAndExit("-m argument expects value");
                    }

                    var modes = args[i + 1];

                    i++;

                    foreach (var c in modes)
                    {
                        ScanPluginBase scan = null;

                        switch (c.ToString().ToLower()[0])
                        {
                            case 'c':
                                scan = new CommandScanPlugin(config.Server);
                                break;
                            case 'l':
                                try
                                {
                                    scan = new LocalFileInclusionScanPlugin(config.Server);
                                }
                                catch (UnauthorizedAccessException)
                                {
                                    ScannerCli.DisplayCriticalMessageAndExit("Error writing LFI test file. Ensure that " +
                                        "PHP Vulnerability Hunter has administrative privileges.");
                                }
                                break;
                            case 'f':
                                scan = new FileScanPlugin(config.Server);
                                break;
                            case 'p':
                                scan = new ArbitraryPhpScanPlugin(config.Server);
                                break;
                            case 's':
                                scan = new SqlScanPlugin(config.Server);
                                break;
                            case 'd':
                                scan = new DynamicScanPlugin(config.Server);
                                break;
                            case 'x':
                                scan = new XssScanPlugin(config.Server);
                                break;
                            case 'i':
                                scan = new FullPathDisclosureScanPlugin(config.Server);
                                break;
                            case 'r':
                                scan = new OpenRedirectScanPlugin(config.Server);
                                break;
                        }

                        if (scan == null)
                            ScannerCli.DisplayCriticalMessageAndExit("Invalid scan mode: " + c);

                        config.ScanPlugins.Add(scan);
                    }
                }
                else
                {
                    switch (argIndex)
                    {
                        case 0:
                            config.WebRoot = args[i];

                            if (!Directory.Exists(config.WebRoot))
                            {
                                ScannerCli.DisplayError(string.Format("Could not find directory {0}",
                                    config.WebRoot));

                                Environment.Exit(5);
                            }

                            break;
                        case 1:
                            if (args[i] == "*")
                            {
                                var dir = new DirectoryInfo(config.WebRoot);
                                config.ApplicationPaths = dir.GetDirectories()
                                    .Select(x => x.Name)
                                    .ToArray();
                            }
                            else
                                config.ApplicationPaths = args[i].Split(',');
                            break;
                    }

                    argIndex++;
                }
            }

            if (argIndex != 2)
                ScannerCli.DisplayCriticalMessageAndExit("Invalid argument count");

            // Validate user input

            if (!Directory.Exists(config.WebRoot))
                ScannerCli.DisplayCriticalMessageAndExit("Web root {0} not found.", config.WebRoot);

            if (config.ScanPlugins.Count == 0 && !config.Repair)
            {
                LocalFileInclusionScanPlugin lfi = null;

                try
                {
                    lfi = new LocalFileInclusionScanPlugin(config.Server);
                }
                catch (UnauthorizedAccessException)
                {
                    ScannerCli.DisplayCriticalMessageAndExit("Error writing LFI test file. Ensure that " +
                        "PHP Vulnerability Hunter has administrative privileges.");
                }

                config._ScanPlugins = new List<ScanPluginBase>()
                {
                    new CommandScanPlugin(config.Server),
                    new FileScanPlugin(config.Server),
                    lfi,                    
                    new ArbitraryPhpScanPlugin(config.Server),
                    new DynamicScanPlugin(config.Server),
                    new SqlScanPlugin(config.Server),                    
                    new XssScanPlugin(config.Server),
                    new OpenRedirectScanPlugin(config.Server),
                    new FullPathDisclosureScanPlugin(config.Server),
                };
            }

            return config;
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
