using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using NavigationService.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing.OptionDetails;
using static NavigationService.PageProperties;

namespace NavigationService
{
    public class NavigationService
    {
        public static bool _allowNavigationViewSelectionChangedNavigation;
        public static bool _allowBreadcrumbBarItemClickedRedirection;
        public static bool _allowFrameNavigatedRedirection;
        public enum NavigateAnimationType
        {
            NoAnimation,
            Entrance,
            DrillIn,
            SlideFromLeft,
            SlideFromRight
        }

        public static NavigationView MainNavigation { get;  set; }
        public static BreadcrumbBar MainBreadcrumb { get;  set; }
        public static Frame MainFrame { get;  set; }


        public static ObservableCollection<Breadcrumb> BreadCrumbs = new ObservableCollection<Breadcrumb>();

        public static ObservableCollection<ObservableCollection<Breadcrumb>> NavigationHistory = new ObservableCollection<ObservableCollection<Breadcrumb>>();

        public static void Initialize(NavigationView navigationView, BreadcrumbBar breadcrumbBar, Frame frame, bool AllowNavigationViewSelectionChangedNavigation = true, bool AllowBreadcrumbBarItemClickedRedirection = true, bool AllowFrameNavigatedRedirection = true)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar;
            MainFrame = frame;

            BreadCrumbs = new ObservableCollection<Breadcrumb>();
            _allowNavigationViewSelectionChangedNavigation = AllowNavigationViewSelectionChangedNavigation;
            _allowBreadcrumbBarItemClickedRedirection = AllowBreadcrumbBarItemClickedRedirection;
            _allowFrameNavigatedRedirection = AllowFrameNavigatedRedirection;

            if (_allowNavigationViewSelectionChangedNavigation)
            {
                navigationView.SelectionChanged += NavigationView_SelectionChanged;
            }
            if (_allowBreadcrumbBarItemClickedRedirection)
            {
                breadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;
            }
            if (_allowFrameNavigatedRedirection)
            {
                frame.Navigated += Frame_Navigated;
            }
        }

        public static void Initialize(NavigationView navigationView, NavigationBreadcrumbBar breadcrumbBar, Frame frame, bool AllowNavigationViewSelectedItemRedirection = true, bool AllowBreadcrumbBarItemClickedRedirection = true, bool AllowFrameNavigatedRedirection = true)
        {
            MainNavigation = navigationView;
            MainBreadcrumb = breadcrumbBar.RootBreadcrumbBar;
            MainFrame = frame;

            BreadCrumbs = new ObservableCollection<Breadcrumb>();
            _allowNavigationViewSelectionChangedNavigation = AllowNavigationViewSelectedItemRedirection;
            _allowBreadcrumbBarItemClickedRedirection = AllowBreadcrumbBarItemClickedRedirection;
            _allowFrameNavigatedRedirection = AllowFrameNavigatedRedirection;

            if (_allowNavigationViewSelectionChangedNavigation)
            {
                navigationView.SelectionChanged += NavigationView_SelectionChanged;
            }
            if (_allowBreadcrumbBarItemClickedRedirection)
            {
                breadcrumbBar.RootBreadcrumbBar.ItemClicked += BreadcrumbBar_ItemClicked;
            }
            if (_allowFrameNavigatedRedirection)
            {
                frame.Navigated += Frame_Navigated;
            }
        }

        private static void Frame_Navigated(object sender, Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
        }

        private static void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            NavigateFromBreadcrumb(args);
        }

        private static void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (_allowNavigationViewSelectionChangedNavigation)
            {
                if (args.IsSettingsSelected)
                {
                    Type target = NavigationProperties.GetSettingsPageTypeProperty(MainNavigation);

                    if (target != null)
                    {
                        Navigate(target, NavigateAnimationType.Entrance);
                    }

                    return; //return when done
                }
                else if (args.SelectedItem != null && args.SelectedItem.GetType() == typeof(NavigationViewItem))
                {
                    NavigationViewItem item = (NavigationViewItem)args.SelectedItem;
                    Type target = NavigationProperties.GetTargetPageTypePropertyProperty(item);
                    NavigateAnimationType anim = NavigationProperties.GetNavigateAnimationTypeProperty(item);

                    if (target != null && anim != null)
                    {
                        Navigate(target, anim);
                    }

                    return; //return when done
                }
            }
        }

        private static void UpdateBreadcrumb()
        {
            MainBreadcrumb.ItemsSource = BreadCrumbs;
        }
        public static void Navigate(Type TargetPageType, NavigateAnimationType AnimType = NavigateAnimationType.Entrance, object parameter = null)
        {
            //prepare all variables
            bool ClearNavigation = true;
            string PageTitle = "";
            bool IsHeaderVisible = true;

            bool AllowNavigationViewItemFocus = false;
            string NavigationViewItemName = null;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            PageTitle = GetPageTitleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            AllowNavigationViewItemFocus = GetAllowNavigationViewItemFocusWhenNavigatedToProperty(obj);
            NavigationViewItemName= GetNavigationViewItemNameProperty(obj);

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
            if (ClearNavigation)
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
                info = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            }

            MainNavigation.AlwaysShowHeader = IsHeaderVisible;
            ChangeBreadcrumbVisibility(IsHeaderVisible);

            MainFrame.Navigate(TargetPageType, parameter, info);

            if (AllowNavigationViewItemFocus)
            {
                SelectNavigationViewItem(NavigationViewItemName);
            }
        }

        public static void NavigateFromBreadcrumb(Type TargetPageType, int BreadcrumbBarIndex, bool NavigatingBackwardsFromBreadcrumb = true)
        {
            //prepare all variables
            bool ClearNavigation = true;
            bool IsHeaderVisible = true;

            bool AllowNavigationViewItemFocus = false;
            string NavigationViewItemName = null;

            DependencyObject obj = Activator.CreateInstance(TargetPageType) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            AllowNavigationViewItemFocus = GetAllowNavigationViewItemFocusWhenNavigatedToProperty(obj);
            NavigationViewItemName = GetNavigationViewItemNameProperty(obj);
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

            if (AllowNavigationViewItemFocus)
            {
                SelectNavigationViewItem(NavigationViewItemName);
            }
        }

        public static void NavigateFromBreadcrumb(BreadcrumbBarItemClickedEventArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args was null");
            }
            if (args.Index < NavigationService.BreadCrumbs.Count)
            {
                //throw new Exception(args.Index + "idk"+ NavigationService.BreadCrumbs.Count.ToString());
            }
            //prepare all variables
            bool ClearNavigation = true;
            bool IsHeaderVisible = true;

            bool AllowNavigationViewItemFocus = false;
            string NavigationViewItemName = null;

            DependencyObject obj = Activator.CreateInstance((args.Item as Breadcrumb).Page) as DependencyObject;

            //get all variables
            IsHeaderVisible = GetIsHeaderVisibleProperty(obj);
            ClearNavigation = GetClearNavigationProperty(obj);

            AllowNavigationViewItemFocus = GetAllowNavigationViewItemFocusWhenNavigatedToProperty(obj);
            NavigationViewItemName = GetNavigationViewItemNameProperty(obj);
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

            if (AllowNavigationViewItemFocus)
            {
                SelectNavigationViewItem(NavigationViewItemName);
            }
        }

        private static void SelectNavigationViewItem(string Name)
        {
            MainNavigation.SelectionChanged -= NavigationView_SelectionChanged;

            //find the item index
            foreach (var item in MainNavigation.MenuItems)
            {
                if (item.GetType() == typeof(NavigationViewItem))
                {
                    NavigationViewItem requireditem = item as NavigationViewItem;

                    if (requireditem.Name == Name)
                    {
                        MainNavigation.SelectedItem = MainNavigation.MenuItems.ElementAt(MainNavigation.MenuItems.IndexOf(requireditem));
                    }
                }
            }

            MainNavigation.SelectionChanged += NavigationView_SelectionChanged;
        }

        public static void ChangeBreadcrumbVisibility(bool IsBreadcrumbVisible)
        {
            if (IsBreadcrumbVisible)
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
                MainNavigation.AlwaysShowHeader = true;
            }
            else
            {
                MainBreadcrumb.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
                MainNavigation.AlwaysShowHeader = false;
            }
        }
    }
}
