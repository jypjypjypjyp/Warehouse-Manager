using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace MyWMS.Helpers
{
    public static class FindChildrenHelper
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject rootObject) where T : DependencyObject
        {
            if (rootObject != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(rootObject); i++)
                {
                    var child = VisualTreeHelper.GetChild(rootObject, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (var childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
            }
        }
    }
}
