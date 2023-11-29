using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NavigationService.NavigationService;

namespace NavigationService
{
    public class NavigationProperties
    {
        public static readonly DependencyProperty TargetPageTypeProperty =
                DependencyProperty.RegisterAttached("TargetPageType", typeof(Type), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetTargetPageTypePropertyProperty(DependencyObject obj, Type value)
        {
            obj.SetValue(TargetPageTypeProperty, value);
        }

        public static Type GetTargetPageTypePropertyProperty(DependencyObject obj)
        {
            return (Type)obj.GetValue(TargetPageTypeProperty);
        }

        public static readonly DependencyProperty SettingsPageTypeProperty =
        DependencyProperty.RegisterAttached("SettingsPageType", typeof(Type), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetSettingsPageTypeProperty(DependencyObject obj, Type value)
        {
            obj.SetValue(SettingsPageTypeProperty, value);
        }

        public static Type GetSettingsPageTypeProperty(DependencyObject obj)
        {
            return (Type)obj.GetValue(SettingsPageTypeProperty);
        }

        public static readonly DependencyProperty NavigateAnimationTypeProperty =
        DependencyProperty.RegisterAttached("NavigateAnimationType", typeof(NavigateAnimationType), typeof(NavigationProperties), new PropertyMetadata(null));

        public static void SetNavigateAnimationTypeProperty(DependencyObject obj, NavigateAnimationType value)
        {
            obj.SetValue(NavigateAnimationTypeProperty, value);
        }

        public static NavigateAnimationType GetNavigateAnimationTypeProperty(DependencyObject obj)
        {
            return (NavigateAnimationType)obj.GetValue(NavigateAnimationTypeProperty);
        }
    }
}
