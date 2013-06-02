using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;

namespace AutoSecTools.Components.Wpf
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private FrameworkElement _element;

        public FrameworkElement Element
        {
            get { return _element; }
            set
            {
                _element = value;

                InvokePropertyChanged("Element");
            }
        }

        private BaseViewModel _parent;

        public BaseViewModel Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;

                InvokePropertyChanged("Parent");
            }
        }

        public BaseViewModel(FrameworkElement Element, BaseViewModel Parent)
        {
            _element = Element;
            _parent = Parent;

            CommandWireup.Wireup(this, _element);
        }

        public BaseViewModel(FrameworkElement Element)
        {
            _element = Element;

            CommandWireup.Wireup(this, _element);
        }

        public BaseViewModel()
        {
        }

        public void InvokePropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        public void InvokePropertyChanged(params string[] PropertyNames)
        {
            if (PropertyChanged != null)
                foreach (var n in PropertyNames)
                    PropertyChanged(this, new PropertyChangedEventArgs(n));
        }
    }
}
