using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LatteLocator.Controls;
using LatteLocator.Core.Common;
using LatteLocator.Views;

namespace LatteLocator
{
    public sealed partial class AppShell : Page
    {
        private ObservableCollection<PaneMenuItem> paneMenuItems;
        public ObservableCollection<PaneMenuItem> PaneMenuItems
        {
            get => paneMenuItems ?? (paneMenuItems = GetMenuItems());
            set => paneMenuItems = value;
        }

        private SystemNavigationManager navManager = null;
        public static AppShell Current = null;
        public AppShell()
        {
            this.InitializeComponent();

            var view = ApplicationView.GetForCurrentView();
            view.TitleBar.BackgroundColor = (App.Current.Resources["LightCoffeeForegroundBrush"] as SolidColorBrush).Color;
            view.TitleBar.ForegroundColor = Colors.White;
            view.TitleBar.ButtonBackgroundColor = (App.Current.Resources["LightCoffeeForegroundBrush"] as SolidColorBrush).Color;
            view.TitleBar.ButtonForegroundColor = Colors.White;
            view.TitleBar.ButtonHoverBackgroundColor = (App.Current.Resources["LightCoffeeForegroundBrush"] as SolidColorBrush).Color;
            view.TitleBar.ButtonHoverForegroundColor = Colors.White;
            view.TitleBar.ButtonPressedBackgroundColor = (App.Current.Resources["StarLightBrush"] as SolidColorBrush).Color;
            view.TitleBar.ButtonPressedForegroundColor = Colors.White;

            Loaded += AppShell_Loaded;
            KeyDown += AppShell_KeyDown;

            navManager = SystemNavigationManager.GetForCurrentView();
            navManager.BackRequested += SystemNavigationManager_BackRequested;

            NavMenuList.ItemsSource = PaneMenuItems;
        }

        private void AppShell_Loaded(object sender, RoutedEventArgs e)
        {
            Current = this;
            TogglePaneButton.Focus(FocusState.Programmatic);
        }

        public Frame AppFrame => this.frame;

        private void AppShell_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            FocusNavigationDirection direction = FocusNavigationDirection.None;
            switch (e.Key)
            {
                case Windows.System.VirtualKey.Left:
                case Windows.System.VirtualKey.GamepadDPadLeft:
                case Windows.System.VirtualKey.GamepadLeftThumbstickLeft:
                case Windows.System.VirtualKey.NavigationLeft:
                    direction = FocusNavigationDirection.Left;
                    break;
                case Windows.System.VirtualKey.Right:
                case Windows.System.VirtualKey.GamepadDPadRight:
                case Windows.System.VirtualKey.GamepadLeftThumbstickRight:
                case Windows.System.VirtualKey.NavigationRight:
                    direction = FocusNavigationDirection.Right;
                    break;

                case Windows.System.VirtualKey.Up:
                case Windows.System.VirtualKey.GamepadDPadUp:
                case Windows.System.VirtualKey.GamepadLeftThumbstickUp:
                case Windows.System.VirtualKey.NavigationUp:
                    direction = FocusNavigationDirection.Up;
                    break;

                case Windows.System.VirtualKey.Down:
                case Windows.System.VirtualKey.GamepadDPadDown:
                case Windows.System.VirtualKey.GamepadLeftThumbstickDown:
                case Windows.System.VirtualKey.NavigationDown:
                    direction = FocusNavigationDirection.Down;
                    break;
            }

            if (direction != FocusNavigationDirection.None)
            {
                var control = FocusManager.FindNextFocusableElement(direction) as Control;
                if (control != null)
                {
                    control.Focus(FocusState.Programmatic);
                    e.Handled = true;
                }
            }
        }

        #region BackRequested Handlers

        private async void SystemNavigationManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (App.ViewModel.IsDirty)
            {
                var md = new MessageDialog("You have unsaved work, if you leave it will not be saved.", "Warning - Do you want to leave this page?");
                md.Commands.Add(new UICommand("yes, discard my work", (a) =>
                {
                    App.ViewModel.IsDirty = false;
                    bool handled = e.Handled;
                    this.BackRequested(ref handled);
                    e.Handled = handled;
                }));
                md.Commands.Add(new UICommand("no, I'll stay", (a) =>
                {
                    e.Handled = true;
                    return;
                }));
                md.CancelCommandIndex = 1;
                md.DefaultCommandIndex = 1;

                await md.ShowAsync();
            }
            else
            {
                bool handled = e.Handled;
                this.BackRequested(ref handled);
                e.Handled = handled;
            }
        }

        private void BackRequested(ref bool handled)
        {
            if (this.AppFrame == null)
                return;

            if (this.AppFrame.CanGoBack && !handled)
            {
                handled = true;
                this.AppFrame.GoBack();
            }
        }

        #endregion

        #region Navigation

        private void NavMenuList_ItemInvoked(object sender, ListViewItem listViewItem)
        {
            var item = (PaneMenuItem)((NavMenuListView)sender).ItemFromContainer(listViewItem);

            if (item?.DestinationType != null && item.DestinationType != AppFrame.CurrentSourcePageType)
            {
                this.AppFrame.Navigate(item.DestinationType, item.Arguments);
            }
        }

        private void OnNavigatingToPage(object sender, NavigatingCancelEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.Back)
            {
                var item = (from p in this.PaneMenuItems where p.DestinationType == e.SourcePageType select p).SingleOrDefault();

                if (item == null && this.AppFrame.BackStackDepth > 0)
                {
                    // In cases where a page drills into sub-pages then we'll highlight the most recent
                    // navigation menu item that appears in the BackStack
                    foreach (var entry in this.AppFrame.BackStack.Reverse())
                    {
                        item = (from p in this.PaneMenuItems where p.DestinationType == entry.SourcePageType select p).SingleOrDefault();
                        if (item != null)
                            break;
                    }
                }

                var container = (ListViewItem)NavMenuList.ContainerFromItem(item);

                // While updating the selection state of the item prevent it from taking keyboard focus.  If a
                // user is invoking the back button via the keyboard causing the selected nav menu item to change
                // then focus will remain on the back button.
                if (container != null) container.IsTabStop = false;
                NavMenuList.SetSelectedItem(container);
                if (container != null) container.IsTabStop = true;
            }
        }

        private void OnNavigatedToPage(object sender, NavigationEventArgs e)
        {
            if (e.Content is Page && e.Content != null)
            {
                var control = (Page)e.Content;
                control.Loaded += Page_Loaded;
            }

            if (!ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                navManager.AppViewBackButtonVisibility = AppFrame.CanGoBack
                ? AppViewBackButtonVisibility.Visible
                : AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ((Page)sender).Focus(FocusState.Programmatic);
            ((Page)sender).Loaded -= Page_Loaded;
        }

        #endregion

        public Rect TogglePaneButtonRect
        {
            get;
            private set;
        }

        /// <summary>
        /// An event to notify listeners when the hamburger button may occlude other content in the app.
        /// The custom "PageHeader" user control is using this.
        /// </summary>
        public event TypedEventHandler<AppShell, Rect> TogglePaneButtonRectChanged;

        /// </summary>
        private void TogglePaneButton_Checked(object sender, RoutedEventArgs e)
        {
            this.CheckTogglePaneButtonSizeChanged();
        }

        private void CheckTogglePaneButtonSizeChanged()
        {
            if (this.RootSplitView.DisplayMode == SplitViewDisplayMode.Inline ||
                this.RootSplitView.DisplayMode == SplitViewDisplayMode.Overlay)
            {
                var transform = this.TogglePaneButton.TransformToVisual(this);
                var rect = transform.TransformBounds(new Rect(0, 0, this.TogglePaneButton.ActualWidth, this.TogglePaneButton.ActualHeight));
                this.TogglePaneButtonRect = rect;
            }
            else
            {
                this.TogglePaneButtonRect = new Rect();
            }

            var handler = this.TogglePaneButtonRectChanged;
            handler?.DynamicInvoke(this, this.TogglePaneButtonRect);
        }

        private void NavMenuItemContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            if (!args.InRecycleQueue && args.Item != null && args.Item is PaneMenuItem)
            {
                args.ItemContainer.SetValue(AutomationProperties.NameProperty, ((PaneMenuItem)args.Item).Label);
            }
            else
            {
                args.ItemContainer.ClearValue(AutomationProperties.NameProperty);
            }
        }

        public static ObservableCollection<PaneMenuItem> GetMenuItems()
        {
            var list = new ObservableCollection<PaneMenuItem>
            {
                new PaneMenuItem {GlyphIcon = Constants.HomeGlyph, Label = "Home", DestinationType = typeof (HomeView)},
                new PaneMenuItem {GlyphIcon = Constants.MapGlyph, Label = "Cards", DestinationType = typeof (CardsView)},
                new PaneMenuItem {GlyphIcon = Constants.StoreGlyph, Label = "Map", DestinationType = typeof (MapView)},
                new PaneMenuItem {GlyphIcon = Constants.SettingsGlyph, Label = "Settings", DestinationType = typeof (SettingsView)},
                new PaneMenuItem {GlyphIcon = Constants.AboutGlyph, Label = "About", DestinationType = typeof (AboutView)}
            };
            return list;
        }
    }
}
