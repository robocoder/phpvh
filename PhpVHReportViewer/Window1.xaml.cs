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
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PhpVH;
using PHPVHReportViewer.ViewModels;

namespace PHPVHReportViewer
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            

            var args = Environment.GetCommandLineArgs();

            if (args.Length > 1)
                OpenFile(args[1]);
            else
                OpenFile();
        }

        public void OpenFile(string File)
        {
            var vm = new ScanViewModel(this);
            vm.OpenFile(File);
            DataContext = vm;
            
            //if (File == null)
            //{
            //    var dialog = new OpenFileDialog() { Filter = "Scan Files (*.pxml)|*.pxml|All|*.*" };
            //    if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            //        return;

            //    File = dialog.FileName;
            //}

            //var alerts = ScanAlertCollection.Load(File);

            //if (alerts == null)
            //    return;

            //var vm = new ScanViewModel(this);
            //vm.CreateAlertViewModels(alerts);

            //DataContext = vm;
        }

        public void OpenFile()
        {
            OpenFile(null);
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFile();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        //{
        //    (DataContext as ScanViewModel).CurrentAlert.UpdateCalls();
        //}

        //private void SetInputMapExpansion(bool isExpanded)
        //{
        //    var style = new Style(typeof(TreeViewItem));
        //    style.Setters.Add(new Setter(TreeViewItem.IsExpandedProperty, isExpanded));
        //    InputTreeView.ItemContainerStyle = style;            
        //}

        //private void ExpandAll_Click(object sender, RoutedEventArgs e)
        //{
        //    SetInputMapExpansion(true);
        //}

        //private void CollapseAll_Click(object sender, RoutedEventArgs e)
        //{
        //    SetInputMapExpansion(false);
        //}
    }
}
