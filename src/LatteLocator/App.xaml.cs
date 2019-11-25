using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using LatteLocator.Core.Common;
using LatteLocator.ViewModels;
using LatteLocator.Views;
using Microsoft.ApplicationInsights;

namespace LatteLocator
{
    sealed partial class App : Application
    {
        private static MainViewModel _viewModel;
        public static MainViewModel ViewModel
        {
            get
            {
                if (_viewModel == null)
                {
                    if (SuspensionManager.SessionState.ContainsKey("MainViewModel"))
                    {
                        Debug.WriteLine("---NOTE---- viewModel was null, but SessionState contains MainViewModel");
                        _viewModel = Core.Common.Helpers.Deserialize<MainViewModel>(SuspensionManager.SessionState["MainViewModel"] as string);
                    }
                    else
                    {
                        Debug.WriteLine("---NOTE---- viewModel was null, returning new MainViewModel");
                        _viewModel = new MainViewModel();
                    }

                    //Application.Current.Resources["MainViewModel"] = _viewModel;

                    return _viewModel;
                }

                return _viewModel;
            }
        }

        //public static TelemetryClient TelemetryClient;
        public static bool IsSuspended;
        public static DisplayRequest DRequest;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            this.UnhandledException += App_UnhandledException;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            var shell = CreateRootShell();

            RestoreStatus(e.PreviousExecutionState);

            if (shell.AppFrame.Content == null)
                shell.AppFrame.Navigate(typeof(HomeView), e.Arguments, new SuppressNavigationTransitionInfo());

            ApplicationView.PreferredLaunchViewSize = new Size { Width = 900, Height = 600 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;

            var applicationView = ApplicationView.GetForCurrentView();
            applicationView.SetDesiredBoundsMode(ApplicationViewBoundsMode.UseCoreWindow);
            applicationView.SetPreferredMinSize(new Size { Width = 360, Height = 533.33 });

            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await StatusBar.GetForCurrentView().HideAsync();
            }

            Window.Current.Activate();
        }

        protected override void OnActivated(IActivatedEventArgs args)
        {

            switch (args.PreviousExecutionState)
            {
                case ApplicationExecutionState.NotRunning:
                    break;
                case ApplicationExecutionState.Running:
                    break;
                case ApplicationExecutionState.Suspended:
                    break;
                case ApplicationExecutionState.Terminated:
                    break;
                case ApplicationExecutionState.ClosedByUser:
                    break;
            }

            base.OnActivated(args);
        }

        private AppShell CreateRootShell()
        {
            AppShell shell = Window.Current.Content as AppShell;

            if (shell == null)
            {
                shell = new AppShell();
                shell.Language = ApplicationLanguages.Languages[0];
                shell.AppFrame.NavigationFailed += OnNavigationFailed;
            }

            Window.Current.Content = shell;

            return shell;
        }

        private async void RestoreStatus(ApplicationExecutionState previousExecutionState)
        {
            Debug.WriteLine("RESTORE STATUS from " + previousExecutionState);

            IsSuspended = false;

            if (previousExecutionState == ApplicationExecutionState.Terminated)
            {
                // Restore the saved session state only when appropriate
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException ex)
                {
                    Debug.WriteLine("Suspension Manager Error: " + ex.Message);
                }
            }
            else if (previousExecutionState == ApplicationExecutionState.Running)
            {
            }
            else if (previousExecutionState == ApplicationExecutionState.NotRunning)
            {
            }
            else if (previousExecutionState == ApplicationExecutionState.ClosedByUser)
            {
                //user closed, no need for last instance
            }
            else if (previousExecutionState == ApplicationExecutionState.Suspended)
            {
                try
                {
                    await SuspensionManager.RestoreAsync();
                }
                catch (SuspensionManagerException ex)
                {
                    Debug.WriteLine("Suspension Manager Error: " + ex.Message);
                }
            }
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            Debug.WriteLine("-------------OnSuspending-------------------");

            IsSuspended = true;

            if (ViewModel != null)
            {
                SuspensionManager.SessionState["MainViewModel"] = Core.Common.Helpers.Serialize(ViewModel);
            }

            await SuspensionManager.SaveAsync();

            if (DRequest != null)
            {
                DRequest.RequestRelease();
                DRequest = null;
                Debug.WriteLine("Display release requested in app.xaml.cs");
            }

            deferral.Complete();
        }

        #region Error Handling

        private async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TelemetryClient.TrackException(e.Exception);

            e.Handled = true;
            ExceptionLogger.LogException(e.Exception);

            var message = "Sorry, there has been an unexpected error. If you'd like to send a techincal summary to the app development team, click Yes.";
            var md = new MessageDialog(message, "Unexpected Error");
            md.Commands.Add(new UICommand("yes", async delegate
            {
                var text = await DiagnosticsHelper.Dump(e.Exception, false);
                await ReportErrorMessage(text);
                //DataTransferManager.ShowShareUI();
            }));
            await md.ShowAsync();
        }

        private async Task<bool> ReportErrorMessage(string detailedErrorMessage)
        {
            var uri = new Uri(string.Format("mailto:awesome.apps@outlook.com?subject=Latte Locator UWP Error&body={0}", detailedErrorMessage), UriKind.Absolute);

            var options = new Windows.System.LauncherOptions
            {
                DesiredRemainingView = ViewSizePreference.UseHalf,
                DisplayApplicationPicker = true,
                PreferredApplicationPackageFamilyName = "microsoft.windowscommunicationsapps_8wekyb3d8bbwe",
                PreferredApplicationDisplayName = "Mail"
            };

            return await Windows.System.Launcher.LaunchUriAsync(uri, options);
        }

        #endregion
    }
}
