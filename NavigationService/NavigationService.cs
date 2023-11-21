using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NavigationService.NavigationProperties;

namespace NavigationService
{
    public class NavigationService
    {
        public enum NavigateAnimationType
        {
            NoAnimation,
            Entrance,
            DrillIn,
            SlideFromLeft,
            SlideFromRight
        }

        private static NavigationView MainNavigation { get;  set; }
        private static BreadcrumbBar MainBreadcrumb { get;  set; }
        private static Frame MainFrame { get;  set; }


        public static ObservableCollection<Breadcrumb> BreadCrumbs = new ObservableCollection<Breadcrumb>();

        public static ObservableCollection<ObservableCollection<Breadcrumb>> NavigationHistory = new ObservableCollection<ObservableCollection<Breadcrumb>>();

        public static void Initialize(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame, bool EnableHistory = false)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;

            BreadCrumbs = new ObservableCollection<Breadcrumb>();
        }

        private static void UpdateBreadcrumb()
        {
            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }
        public static void Navigate(Type TargetPageType, NavigateAnimationType AnimType, object parameter = null, bool NavigatingBackwardsFromBreadcrumb = false)
        {
            //prepare all variables
            bool ClearNavigation = true;
            string PageTitle = "";
            bool IsHeaderVisible = true;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            PageTitle = GetPageTitleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            //prepare navigation

            //prepare breadcrumbs
            if (ClearNavigation)
            {
                BreadCrumbs.Clear();
                MainFrame.BackStack.Clear();
            }
            BreadCrumbs.Add(new Breadcrumb(PageTitle, TargetPageType));
            UpdateBreadcrumb();

            //prepare transtions
            NavigationTransitionInfo info;
            if (!NavigatingBackwardsFromBreadcrumb)
            {
                switch (AnimType)
                {
                    case NavigateAnimationType.NoAnimation:
                        info = new SuppressNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.Entrance:
                        info = new EntranceNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.DrillIn:
                        info = new DrillInNavigationTransitionInfo();
                        break;
                    case NavigateAnimationType.SlideFromRight:
                        info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
                        break;
                    case NavigateAnimationType.SlideFromLeft:
                        info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft };
                        break;
                    default:
                        info = new EntranceNavigationTransitionInfo();
                        break;
                }
            }
            else
            {
                info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft };
            }

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            MainFrame.Navigate(TargetPageType, parameter, info);
        }

        public static void NavigateFromBreadcrumb(Type TargetPageType, int BreadcrumbBarIndex, bool NavigatingBackwardsFromBreadcrumb = true)
        {
            //prepare all variables
            bool ClearNavigation = true;
            bool IsHeaderVisible = true;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            //prepare navigation

            //prepare transtions

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate(TargetPageType, null, info);

            int indexToRemoveAfter = BreadcrumbBarIndex;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter - 1;

                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                }
            }
        }

        public static void NavigateFromBreadcrumb(BreadcrumbBarItemClickedEventArgs args)
        {
            if (args == null || args.Index < NavigationService.BreadCrumbs.Count - 1)
            {
                throw new ArgumentNullException("args was null");
            }
            //prepare all variables
            bool ClearNavigation = true;
            bool IsHeaderVisible = true;

            DependencyObject obj = Activator.CreateInstance((args.Item as Breadcrumb).Page) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            //prepare navigation

            //prepare transtions

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            SlideNavigationTransitionInfo info = new SlideNavigationTransitionInfo();
            info.Effect = SlideNavigationTransitionEffect.FromLeft;
            MainFrame.Navigate((args.Item as Breadcrumb).Page, null, info);

            int indexToRemoveAfter = args.Index;

            if (indexToRemoveAfter < BreadCrumbs.Count - 1)
            {
                int itemsToRemove = BreadCrumbs.Count - indexToRemoveAfter - 1;

                for (int i = 0; i < itemsToRemove; i++)
                {
                    BreadCrumbs.RemoveAt(indexToRemoveAfter + 1);
                }
            }
        }

        public static void ChangeBreadcrumbVisibility(bool IsBreadcrumbVisible)
        {
            if (IsBreadcrumbVisible)
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }
            else
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
            }
        }
    }
}
