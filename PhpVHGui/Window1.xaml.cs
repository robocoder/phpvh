using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Diagnostics;
using PhpVH;
using System.Threading;
using System.Reflection;

namespace PHPVHGUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        string _configFile = new FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName + 
            "\\config.xml";

        string _phpvhExe;

        private ScanConfig _Config;
        
        public ScanConfig Config
        {
        	get { return _Config; }
        	set
        	{                
        		_Config = value;
        
        		if (PropertyChanged != null)
        			PropertyChanged(this, new PropertyChangedEventArgs("Config"));

                if (_Config.StaticOnly)
                {
                    HybridAnalysis = false;
                    StaticAnalysis = true;
                }
                else
                {
                    HybridAnalysis = true;
                    StaticAnalysis = false;
                }
        	}
        }

        private bool _HybridAnalysis;
        
        public bool HybridAnalysis
        {
        	get { return _HybridAnalysis; }
        	set
        	{                
        		_HybridAnalysis = value;
                _StaticAnalysis = !value;

                OnAnalysisModeChanged();
        	}
        }

        private bool _StaticAnalysis;
        
        public bool StaticAnalysis
        {
        	get { return _StaticAnalysis; }
        	set
        	{                
        		_StaticAnalysis = value;
                _HybridAnalysis = !value;

                OnAnalysisModeChanged();
        	}
        }

        private Visibility _HybridAnalysisVisibility = Visibility.Visible;
        
        public Visibility HybridAnalysisVisibility
        {
        	get { return _HybridAnalysisVisibility; }
        	set
        	{                
        		_HybridAnalysisVisibility = value;
        
        		if (PropertyChanged != null)
        			PropertyChanged(this, new PropertyChangedEventArgs("HybridAnalysisVisibility"));
        	}
        }

        private System.Windows.Forms.FolderBrowserDialog _folderDialog = new System.Windows.Forms.FolderBrowserDialog();

        public Window1()
        {
            InitializeComponent();

            DataContext = this;

            Config = new ScanConfig() { RunViewer = true };            

            Loaded += new RoutedEventHandler(Window1_Loaded);
            Closing += new CancelEventHandler(Window1_Closing);

            var l = Assembly.GetExecutingAssembly().Location;

            _phpvhExe = new FileInfo(l).Directory + "\\phpvh.exe";            

        }

        private void OnAnalysisModeChanged()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("HybridAnalysis"));
                PropertyChanged(this, new PropertyChangedEventArgs("StaticAnalysis"));
            }

            Config.StaticOnly = _StaticAnalysis;
            
            HybridAnalysisVisibility = _HybridAnalysis ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private void SaveConfig()
        {
            Config.Serialize(_configFile);
        }

        private IEnumerable<DirectoryInfo> SearchForPHPDirs(DirectoryInfo TargetDirectory)
        {
            return TargetDirectory
                .GetDirectories()
                .Where(x =>
                {
                    try
                    {
                        return x.GetFiles().Any(y => y.Extension.ToLower() == ".php");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        return false;
                    }
                });
            //foreach (var subDir in TargetDirectory.GetDirectories())
            //{

            //}
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            SizeToContent = SizeToContent.WidthAndHeight;

            if (File.Exists(_configFile))
            {
                Config = ScanConfig.Deserialize(_configFile);
            }

            SetAppsEnabled();            
        }

        void Window1_Closing(object sender, CancelEventArgs e)
        {
            SaveConfig();
        }

        private void ShowDialog(string Message, string Title)
        {
            new DialogWindow()
            {
                Message = Message,
                Owner = this,
                Title = Title,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            }.ShowDialog();
        }

        private bool Validate()
        {
            var messages = Config.Validate();

            if (!File.Exists(_phpvhExe))
                messages.Add("phpvh.exe not found");

            if (messages.Count == 0)
                return true;

            ShowDialog(messages.Aggregate((x, y) => x + "\r\n\r\n" + y),
                "Validation Errors");

            return false;
        }

        private void ShowStartError(Exception ex)
        {
            MessageBox.Show("Error starting phpvh\r\n\r\n" + ex.Message,
                    "Error starting phpvh", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void RunPhpvh(string extraArgs = "", bool exit = false)
        {
            if (!exit)
            {
                IsEnabled = false;
            }

            SaveConfig();

            if (!Validate())
            {
                if (!exit)
                {
                    IsEnabled = true;
                }

                return;
            }

            var args = Config.GetArgs() + " -l " + extraArgs;

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    Verb = "runas",
                    FileName = _phpvhExe,
                    Arguments = args,                    
                },
                
            };

            try
            {
                process.Start();
            }
            catch (Win32Exception ex)
            {
                ShowStartError(ex);

                return;
            }

            if (exit)
            {
                Environment.Exit(0);
            }
            else
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    process.WaitForExit();

                    Dispatcher.Invoke(() => IsEnabled = true);
                });
            }
        }

        private void WebrootBrowseButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(Config.WebRoot))
                _folderDialog.SelectedPath = Config.WebRoot;

            if (_folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Config.WebRoot = _folderDialog.SelectedPath;                
            }
        }

        private void AppsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetAppsEnabled()
        {
            AppsTextBox.IsEnabled = !Config.AllApps;
        }

        private void ScanAllCheckBox_Click(object sender, RoutedEventArgs e)
        {
            SetAppsEnabled();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();

            Environment.Exit(0);
        }

        private void HelpMenuItem_Click(object sender, RoutedEventArgs e)
        {
            new Guide().Show();
            //Process.Start("http://www.autosectools.com/Page/PHP-Vulnerability-Hunter-Guide");
        }

        private void RepairButton_Click(object sender, RoutedEventArgs e)
        {
            RunPhpvh("-r");
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            RunPhpvh("-test", exit: false);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            RunPhpvh();
        }        

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
    }
}
