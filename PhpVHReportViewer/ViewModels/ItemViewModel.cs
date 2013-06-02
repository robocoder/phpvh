using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AutoSecTools.Components.Wpf
{
    public class ItemViewModel<T> : BaseViewModel
        where T : class
    {
        private Visibility _visibility = Visibility.Visible;

        public Visibility Visibility
        {
            get { return _visibility; }
            set { _visibility = value; InvokePropertyChanged("Visibility"); }
        }

        private T _Item;
        
        public T Item
        {
        	get { return _Item; }
        	set
        	{                
        		_Item = value;
        
        		InvokePropertyChanged("Item");
        	}
        }

        public ItemViewModel(T Item, FrameworkElement Element, BaseViewModel Parent)
            : base(Element, Parent)
        {
            _Item = Item;
        }

        public ItemViewModel(T Item, FrameworkElement Element)
            : base(Element)
        {
            _Item = Item;
        }

        public ItemViewModel(T Item)
            : base()
        {
            _Item = Item;
        }
    }
}
