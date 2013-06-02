using Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
using PHPVHReportViewer.Models;
using PhpVH.CodeCoverage;
using System.Security.Cryptography;
using System.Diagnostics;

namespace PHPVHReportViewer.Views
{
    /// <summary>
    /// Interaction logic for DynamicAnalysisDetailsControl.xaml
    /// </summary>
    public partial class DynamicAnalysisDetailsControl : UserControl
    {
        public DynamicAnalysisDetailsControl()
        {
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            var textbox = (sender as Hyperlink).Parent as FrameworkElement;
            
            var coverage = ((KeyValuePair<string, decimal>)textbox.DataContext).Key;

            var md5 = new MD5CryptoServiceProvider().ComputeHash(coverage);
            var item = textbox
                .FindVisualParent<TreeViewItem>()
                .FindVisualParent<TreeViewItem>();

            var table = item.DataContext as CodeCoverageTable;

            var htmlFile = System.IO.Path.Combine(
                "Code Coverage",
                PathHelper.SanitizeName(table.Plugin), 
                md5 + ".html");

            var window = item.FindVisualParent<Window>() as Window;
            var reportDir = (window.DataContext as ViewModels.ScanViewModel).ReportDir;

            Process.Start(System.IO.Path.Combine(reportDir, htmlFile));
        }
    }
}
