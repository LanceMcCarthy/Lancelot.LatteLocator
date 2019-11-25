using LatteLocator.Core.Common;
using System;
using Windows.UI.ViewManagement;

namespace LatteLocator.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            PrivacyPolicyCommand = new DelegateCommand(async () =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("http://w8privacy.azurewebsites.net/privacy?dev=Lancelot%20Software&app=Latte%20Locator&mail=bGFuY2VAb3V0dGFzeXl0ZS5jb20=&lng=En"));
                
            });

            RateThisAppCommand = new DelegateCommand(async () =>
            {
                await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://pdp/?productid=9nblggh0f2cn"));
            });

            SendAnEmailCommand = new DelegateCommand(async () =>
            {
                var uri = new Uri(string.Format("mailto:awesome.apps@outlook.com?subject=Latte Locator Feedback"), UriKind.Absolute);

                var options = new Windows.System.LauncherOptions
                {
                    DesiredRemainingView = ViewSizePreference.UseHalf,
                    DisplayApplicationPicker = true,
                    PreferredApplicationPackageFamilyName = "microsoft.windowscommunicationsapps_8wekyb3d8bbwe",
                    PreferredApplicationDisplayName = "Mail"
                };

                await Windows.System.Launcher.LaunchUriAsync(uri, options);
            });
        }

        public DelegateCommand PrivacyPolicyCommand { get; private set; }
        public DelegateCommand RateThisAppCommand { get; private set; }
        public DelegateCommand SendAnEmailCommand { get; private set; }
        
    }
}
