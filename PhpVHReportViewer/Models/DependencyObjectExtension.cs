using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PHPVHReportViewer.Models
{
    public static class DependencyObjectExtension
    {
        public static T FindVisualParent<T>(this DependencyObject d)
            where T : DependencyObject
        {
            var d2 = VisualTreeHelper.GetParent(d);

            T match = null;

            if (d2 == null)
            {
                return null;
            }
            else if ((match = d2 as T) != null)
            {
                return match;
            }
            else
            {
                return d2.FindVisualParent<T>();
            }
        }
    }
}
