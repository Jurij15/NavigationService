using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace TestApp_.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Page1 : Page
    {
        public Page1()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigationService.Navigate(typeof(Page2));
        }

        private void Page2btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigationService.Navigate(typeof(Page2));
        }

        private void Page3btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigationService.Navigate(typeof(Page3));
        }

        private void Page4btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigationService.Navigate(typeof(Page4));
        }

        private void Page5btn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigationService.Navigate(typeof(Page5));
        }
    }
}
