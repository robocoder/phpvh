using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace AutoSecTools.Components.Wpf
{
    public class CommandWireup
    {    
        public static void Wireup(BaseViewModel ViewModel, UIElement Element)
        {
            Type t = ViewModel.GetType();

            IEnumerable<FieldInfo> commands = t.GetFields().Where(x =>
                x.FieldType == typeof(RoutedCommand));

            foreach (FieldInfo f in commands)
            {
                string name = f.Name.ToLower().EndsWith("command") ?
                    f.Name.Remove(f.Name.Length - 7) : f.Name;

                string executedName = name + "Executed",
                    canExecuteName = name + "CanExecute";

                ExecutedRoutedEventHandler executedDelegate =
                    Delegate.CreateDelegate(typeof(ExecutedRoutedEventHandler),
                        ViewModel, executedName) as ExecutedRoutedEventHandler;
                //executed) as ExecutedRoutedEventHandler;

                CanExecuteRoutedEventHandler canExecuteDelegate = null;

                try
                {
                    canExecuteDelegate =
                        Delegate.CreateDelegate(typeof(CanExecuteRoutedEventHandler),
                            ViewModel, canExecuteName) as CanExecuteRoutedEventHandler;
                }
                catch (ArgumentException) { }

                RoutedCommand command = f.GetValue(ViewModel) as RoutedCommand;

                if (canExecuteDelegate == null)
                {
                    Element.CommandBindings.Add(new CommandBinding(command,
                        executedDelegate));
                }
                else
                {
                    Element.CommandBindings.Add(new CommandBinding(command,
                        executedDelegate, canExecuteDelegate));
                }
            }
        }
    }
}
