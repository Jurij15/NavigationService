using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NavigationService
{
    public class NavigationProperties
    {
        public static readonly DependencyProperty IsHeaderVisibleProperty =
            DependencyProperty.RegisterAttached("IsHeaderVisible", typeof(bool), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetIsHeaderVisibleProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHeaderVisibleProperty, value);
        }

        public static bool GetIsHeaderVisibleProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHeaderVisibleProperty);
        }

        public static readonly DependencyProperty ClearNavigationProperty =
            DependencyProperty.RegisterAttached("ClearNavigation", typeof(bool), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetClearNavigationProperty(DependencyObject obj, bool value)
        {
            obj.SetValue(ClearNavigationProperty, value);
        }

        public static bool GetClearNavigationProperty(DependencyObject obj)
        {
            return (bool)obj.GetValue(ClearNavigationProperty);
        }

        public static readonly DependencyProperty PageTitleProperty =
            DependencyProperty.RegisterAttached("PageTitle", typeof(string), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetPageTitleProperty(DependencyObject obj, string value)
        {
            obj.SetValue(PageTitleProperty, value);
        }

        public static string GetPageTitleProperty(DependencyObject obj)
        {
            return (string)obj.GetValue(PageTitleProperty);
        }
    }
}
