using System;
using System.Diagnostics;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using LatteLocator.Helpers;
using Telerik.UI.Xaml.Controls.Data;

namespace LatteLocator.Views
{
    public sealed partial class CardsView : Page
    {
        public CardsView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ViewModel.LoadCardsCommand.Execute(null);
        }

        private void StarbucksCards_ItemTap(object sender, ListBoxItemTapEventArgs e)
        {
            Frame.Navigate(typeof(CardDetailsView), ViewModel.SelectedCard);
        }

        private void FinalPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (App.ViewModel.IsAppLocked)
            {
                //if app is locked, dont force validation because FirstPasswordBox is hidden
                //FinalPasswordBox.ChangeValidationState(ValidationState.NotValidated, "");
            }
            else
            {
                //FinalPasswordBox.ChangeValidationState(ValidationState.Validating, "Validating");

                if (FinalPasswordBox.Password == InitialPasswordBox.Password)
                {
                    //FinalPasswordBox.ChangeValidationState(ValidationState.Valid, "match");
                }
                else
                {
                    //FinalPasswordBox.ChangeValidationState(ValidationState.Invalid, "no match");
                }
            }
        }

        private async void UseLastPasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var pwd = await PasswordUtilities.GetPasswordAsync();

                if (string.IsNullOrEmpty(pwd))
                {
                    await new MessageDialog("You do not have a stored password. Enter a new one to lock the app", "Create Password").ShowAsync();
                    return;
                }

                InitialPasswordBox.Password = pwd;
                FinalPasswordBox.Password = pwd;
                LockApp();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"CheckPassword(): Exception: {ex.Message}");
            }
        }

        private void UnlockButton_Clicked(object sender, RoutedEventArgs e)
        {
            //If this click happened, the app is always locked. so hide the button
            UseLastPasswordButton.Visibility = Visibility.Collapsed;

            //show the overlay
            PasswordOverlay.Visibility = Visibility.Visible;
        }
        
        private async void LockApp()
        {
            var md = new MessageDialog("You are about to lock your Starbucks cards.", "Continue?");

            md.Commands.Add(new UICommand("LOCK"));
            md.Commands.Add(new UICommand("Cancel"));

            var result = await md.ShowAsync();

            if (result.Label == "LOCK")
            {
                var success = await PasswordUtilities.SavePasswordAsync(FinalPasswordBox.Password);

                if (success)
                {
                    App.ViewModel.IsAppLocked = true;

                    InitialPasswordBox.Password = "";
                    FinalPasswordBox.Password = "";
                    //FinalPasswordBox.ChangeValidationState(ValidationState.NotValidated, "");

                    PasswordOverlay.Visibility = Visibility.Collapsed;
                }
                else
                {
                    await new MessageDialog("Something went wrong trying to set your password, try again.", "Error").ShowAsync();
                }
            }
        }

        private async void UnlockApp()
        {
            var success = await PasswordUtilities.CheckPasswordAsync(FinalPasswordBox.Password);

            if (success)
            {
                App.ViewModel.IsAppLocked = false;

                InitialPasswordBox.Password = "";
                FinalPasswordBox.Password = "";

                PasswordOverlay.Visibility = Visibility.Collapsed;
            }
            else
            {
                await new MessageDialog("Wrong password, try again.", "Try Again").ShowAsync();
            }
        }

        private void PasswordManagerButtonVisibility()
        {
            //if the app is locked or if there isn't a password stored, hide the button
            if (!App.ViewModel.IsAppLocked && !PasswordUtilities.HasPassword())
            {
                UseLastPasswordButton.Visibility = Visibility.Visible;
            }
            else
            {
                UseLastPasswordButton.Visibility = Visibility.Collapsed;
            }
        }

        private void AddButton_Clicked(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CardEditView));
        }
    }
}
