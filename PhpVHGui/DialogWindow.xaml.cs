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
using System.Windows.Shapes;
using System.ComponentModel;

namespace PHPVHGUI
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private string _Message;
        
        public string Message
        {
        	get { return _Message; }
        	set
        	{                
        		_Message = value;
        
        		if (PropertyChanged != null)
        			PropertyChanged(this, new PropertyChangedEventArgs("Message"));
        	}
        }

        public DialogWindow()
        {
            InitializeComponent();

            DataContext = this;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }        
    }
}
