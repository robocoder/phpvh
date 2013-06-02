using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using AutoSecTools.Components.Wpf;
using PhpVH;

namespace PHPVHReportViewer.ViewModels
{
    public class AlertViewModel : ItemViewModel<ScanAlert>
    {
        private bool _NameFilterEnabled;

        public bool NameFilterEnabled
        {
            get { return _NameFilterEnabled; }
            set
            {
                _NameFilterEnabled = value;

                InvokePropertyChanged("NameFilterEnabled");
            }
        }

        private string _NameFilter = "";

        public string NameFilter
        {
            get { return _NameFilter; }
            set
            {
                _NameFilter = value;

                InvokePropertyChanged("NameFilter");
            }
        }

        private bool _ParamFilterEnabled;

        public bool ParamFilterEnabled
        {
            get { return _ParamFilterEnabled; }
            set
            {
                _ParamFilterEnabled = value;

                InvokePropertyChanged("ParamFilterEnabled");
            }
        }

        private string _ParamFilter = "";

        public string ParamFilter
        {
            get { return _ParamFilter; }
            set
            {
                _ParamFilter = value;

                InvokePropertyChanged("ParamFilter");
            }
        }

        private bool _ReturnFilterEnabled;

        public bool ReturnFilterEnabled
        {
            get { return _ReturnFilterEnabled; }
            set
            {
                _ReturnFilterEnabled = value;

                InvokePropertyChanged("ReturnFilterEnabled");
            }
        }

        private string _ReturnFilter = "";

        public string ReturnFilter
        {
            get { return _ReturnFilter; }
            set
            {
                _ReturnFilter = value;

                InvokePropertyChanged("ReturnFilter");
            }
        }

        private ObservableCollection<TracedFunctionCall> _Calls;
        
        public ObservableCollection<TracedFunctionCall> Calls
        {
        	get { return _Calls; }
        	set
        	{                
        		_Calls = value;
        
        		InvokePropertyChanged("Calls");
        	}
        }

        public AlertViewModel(ScanAlert Item)
            : base(Item)
        {
            UpdateCalls();
        }

        public void UpdateCalls()
        {
            IEnumerable<TracedFunctionCall> calls = Item.Trace.Calls;

            if (NameFilterEnabled)
                calls = calls
                    .Where(x => x.Name != null && 
                        x.Name.ToLower().Contains(NameFilter.ToLower()));

            if (ParamFilterEnabled)
                calls = calls
                    .Where(x => x.ParameterValues != null && x.ParameterValues
                        .Any(y => y != null && 
                            y.ToLower().Contains(ParamFilter.ToLower())));

            if (ReturnFilterEnabled)
                calls = calls
                    .Where(x => x.Value != null && 
                        x.Value.ToLower().Contains(ReturnFilter.ToLower()));

            Calls = new ObservableCollection<TracedFunctionCall>(calls);
        }
    }
}
