# NavigationService
A small library to help you navigate in your WinUI3 apps

# Sample
- For sample usage, refer to the [TestApp](https://github.com/Jurij15/NavigationService/tree/master/TestApp%23)

# Properties
## Page Properties
| Name      | Data Type | Description |
| ----------- | ----------- | ----------- |
| IsHeaderVisible | bool | Determines whether NavigationViewHeader will be visible when navigated to this page |
| ClearNavigation | bool | Clear the navigation Breadcrumb will be cleaned when navigated to the page |
| PageTitle | string | Sets the title of the page on Breadcrumb|
| AllowNavigationViewItemFocusWhenNavigatedTo | bool | Focus the required NavigationViewItem when navigated to this page|
| NavigationViewItemName | string | Name of the NavigationViewItem to focus if AllowNavigationViewItemFocusWhenNavigatedTo is set to true |
### Notes
- Only use AllowNavigationViewItemFocusWhenNavigatedTo if ClearNavigation is set to true, otherwise it might look confusing to use

## NavigationViewItem Properties
| Name      | Data Type | Description |
| ----------- | ----------- | ----------- |
| TargetPageType | System.Type | Page to navigate to |
| NavigateAnimationType | NavigationService.NavigateAnimationType | Animation to use when navigating |
| SettingsPageType | System.Type | Settings page to navigate to when IsSettingsVisible is true and is selected |
