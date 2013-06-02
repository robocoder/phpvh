using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using AutoSecTools.Components.Wpf;
using PhpVH;
using PhpVH.StaticAnalysis;
using PhpVH.CodeCoverage;
using System.IO;

namespace PHPVHReportViewer.ViewModels
{
    public class ScanViewModel : BaseViewModel
    {
        private Dictionary<string, Dictionary<string, List<object>>> _inputTable;

        public Dictionary<string, Dictionary<string, List<object>>> InputTable
        {
            get { return _inputTable; }
            set { _inputTable = value; InvokePropertyChanged("InputTable"); }
        }

        private object _StaticAnalysisAlerts;
        
        public object StaticAnalysisAlerts
        {
        	get { return _StaticAnalysisAlerts; }
        	set
        	{                
        		_StaticAnalysisAlerts = value;
        
        		InvokePropertyChanged("StaticAnalysisAlerts");
        	}
        }

        private int _SelectedRootTab = -1;
        
        public int SelectedRootTab
        {
        	get { return _SelectedRootTab; }
        	set
        	{                
        		_SelectedRootTab = value;
        
        		InvokePropertyChanged("SelectedRootTab");
        	}
        }

        private Visibility _DynamicAnalysisVisibility = Visibility.Collapsed;
        
        public Visibility DynamicAnalysisVisibility
        {
        	get { return _DynamicAnalysisVisibility; }
        	set
        	{                
        		_DynamicAnalysisVisibility = value;
        
        		InvokePropertyChanged("DynamicAnalysisVisibility");
        	}
        }

        private Visibility _StaticAnalysisVisibility = Visibility.Collapsed;
        
        public Visibility StaticAnalysisVisibility
        {
        	get { return _StaticAnalysisVisibility; }
        	set
        	{                
        		_StaticAnalysisVisibility = value;
        
        		InvokePropertyChanged("StaticAnalysisVisibility");
        	}
        }

        private int _SelectedTab = -1;
        
        public int SelectedTab
        {
        	get { return _SelectedTab; }
        	set
        	{                
        		_SelectedTab = value;
        
        		InvokePropertyChanged("SelectedTab");
        	}
        }

        private Visibility _VulnerabilityTabVisibility = Visibility.Collapsed;
        
        public Visibility VulnerabilityTabVisibility
        {
        	get { return _VulnerabilityTabVisibility; }
        	set
        	{                
        		_VulnerabilityTabVisibility = value;
        
        		InvokePropertyChanged("VulnerabilityTabVisibility");
        	}
        }

        private Visibility _InputMapVisibility = Visibility.Collapsed;
        
        public Visibility InputMapVisibility
        {
        	get { return _InputMapVisibility; }
        	set
        	{                
        		_InputMapVisibility = value;
        
        		InvokePropertyChanged("InputMapVisibility");
        	}
        }

        private Visibility _CoverageVisibility = Visibility.Collapsed;
        
        public Visibility CoverageVisibility
        {
        	get { return _CoverageVisibility; }
        	set
        	{                
        		_CoverageVisibility = value;
        
        		InvokePropertyChanged("CoverageVisibility");
        	}
        }

        private CodeCoverageTable[] _CoverageTables;

        public CodeCoverageTable[] CoverageTables
        {
        	get { return _CoverageTables; }
        	set
        	{                
        		_CoverageTables = value;
        
        		InvokePropertyChanged("CoverageTables");
        	}
        }

        private ObservableCollection<AlertViewModel> _Alerts;
        
        public ObservableCollection<AlertViewModel> Alerts
        {
        	get { return _Alerts; }
        	set
        	{                
        		_Alerts = value;
        
        		InvokePropertyChanged("Alerts");
        	}
        }

        private AlertViewModel _CurrentAlert;

        public AlertViewModel CurrentAlert
        {
            get { return _CurrentAlert; }
            set
            {
                _CurrentAlert = value;

                InvokePropertyChanged("CurrentAlert");
            }
        }

        private string _ReportDir;

        public string ReportDir
        {
            get { return _ReportDir; }
            set
            {
                _ReportDir = value;

                InvokePropertyChanged("ReportDir");
            }
        }

        public ScanViewModel(FrameworkElement Element, BaseViewModel Parent)
            : base(Element, Parent)
        {
        }

        public ScanViewModel(FrameworkElement Element)
            : base(Element)
        {
            
        }

        public ScanViewModel()
            : base()
        {
        }

        private void CreateAlertViewModels(IEnumerable<ScanAlert> ScanAlerts)
        {
            //var map = ApplicationMap.FromXml(@"C:\Temp\Release\phpvh\source\PHPScan\PHPVH\bin\Debug\ebbv3 Scan Reports 12-30-2011-025756\input map report.xml");


            Alerts = new ObservableCollection<AlertViewModel>(ScanAlerts.Select(x => new AlertViewModel(x)));

            CurrentAlert = Alerts.First();
        }

        public void OpenFile(string SourceFile)
        {
            SelectedTab = -1;

            if (SourceFile == null)
            {
                var dialog = new OpenFileDialog() 
                { 
                    Filter = "Report Files (*.rxml Files)|*.rxml|Vuln Files (*.pxml)|*.pxml|All|*.*" 
                };
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                SourceFile = dialog.FileName;                
            }

            var info = new FileInfo(SourceFile);

            ReportDir = info.Directory.FullName;

            if (info.Extension == ".rxml")
            {
                var reportFiles = ReportFile.Load(SourceFile);

                Func<string, string> getFilePath = x => info.DirectoryName + "\\" + 
                    new FileInfo(x).Name;

                var staticAnalysisReport = reportFiles
                    .SingleOrDefault(x => x.Name == "Static analysis");

                if (staticAnalysisReport != null)
                {
                    var alerts = StaticAnalysisFileAlertCollection
                        .Load(getFilePath(staticAnalysisReport.Filename))
                        .SelectMany(x => x.Alerts.Select(y => new
                        {
                            x.Filename,
                            Alert = y,
                        }))
                        .OrderBy(x => x.Alert.Name);

                    if (alerts.Any())
                    {
                        StaticAnalysisVisibility = Visibility.Visible;
                        StaticAnalysisAlerts = alerts;
                    }
                }

                var vulnReport = reportFiles
                    .SingleOrDefault(x => new FileInfo(x.Filename).Extension == ".pxml");

                if (vulnReport != null)
                {
                    VulnerabilityTabVisibility = Visibility.Visible;
                    SelectedTab = 0;

                    var alerts = ScanAlertCollection.Load(getFilePath(vulnReport.Filename));

                    if (alerts == null)
                        return;

                    CreateAlertViewModels(alerts);
                }
                else
                    VulnerabilityTabVisibility = Visibility.Collapsed;


                var inputMap = reportFiles
                    .SingleOrDefault(x => x.Name == "Input Map Report");

                if (inputMap != null)
                {
                    var inputMapFile = inputMap.Filename;
                    InputTable = ApplicationMap
                        .FromXml(getFilePath(inputMapFile)).Pages
                        .ToDictionary(
                            x => string.Format(
                                "{0} ({1})",
                                x.Page,
                                x.SuperglobalNameCollectionTable.Sum(y => y.Value.Count())),
                            x => new Dictionary<string, List<object>>()
                            {
                                { "GET", x.Get.Select(y => (object)(new { Key = y })).ToList() },
                                { "POST", x.Post.Select(y => (object)(new { Key = y })).ToList() },
                                { "REQUEST", x.Request.Select(y => (object)(new { Key = y })).ToList() },
                                { "Files", x.Files.Select(y => (object)(new { Key = y })).ToList() },
                                { "Cookies", x.Cookie.Select(y => (object)(new { Key = y })).ToList() }

                            }
                            .Where(y => y.Value.Any())
                            .ToDictionary(
                                y => string.Format("{0} ({1})", y.Key, 
                                y.Value.Count()), y => y.Value));

                    InputMapVisibility = Visibility.Visible;
                    if (SelectedTab == -1)
                        SelectedTab = 1;
                }
                

                var coverageReport = reportFiles
                    .SingleOrDefault(x => x.Name == "Annotation");

                if (coverageReport != null)
                {
                    CoverageVisibility = Visibility.Visible;
                    if (SelectedTab == -1)
                        SelectedTab = 2;

                    var annotationFile = Path.Combine(info.DirectoryName, coverageReport.Filename);
                    var pluginTable = PluginAnnotationTable.Load(annotationFile);

                    CoverageTables = pluginTable.Items
                        .Select(x => new CodeCoverageCalculator(x).CalculateCoverage(false))
                        .ToArray();
                }
                else
                    CoverageVisibility = Visibility.Collapsed;

                if (SelectedTab != -1)
                {
                    DynamicAnalysisVisibility = Visibility.Visible;
                    SelectedRootTab = 0;
                }
                else
                {
                    SelectedRootTab = 1;
                }
            }
            else
            {
                var alerts = ScanAlertCollection.Load(SourceFile);

                if (alerts == null)
                    return;

                DynamicAnalysisVisibility = Visibility.Visible;
                SelectedRootTab = 0;
                VulnerabilityTabVisibility = Visibility.Visible;
                InputMapVisibility = Visibility.Collapsed;
                CoverageVisibility = Visibility.Collapsed;
                SelectedTab = 0;

                CreateAlertViewModels(alerts);
            }
        }
    }
}
