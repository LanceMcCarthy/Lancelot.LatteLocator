using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using LatteLocator.Core.Models;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace LatteLocator.Views
{
    public sealed partial class CardDetailsView : Page
    {
        public CardDetailsView()
        {
            this.InitializeComponent();
        }
        
        private void SizeTextBlock_Tap(object sender, TappedRoutedEventArgs e)
        {
            var tb = (TextBlock)sender;

            switch (tb.Text)
            {
                case "S":
                    SmallTextBlock.Foreground = App.Current.Resources["StarGreenBrush"] as SolidColorBrush;
                    MediumTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    LargeTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    ViewModel.PreferredBarcodeZoom = "small";
                    break;
                case "M":
                    SmallTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    MediumTextBlock.Foreground = App.Current.Resources["StarGreenBrush"] as SolidColorBrush;
                    LargeTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    ViewModel.PreferredBarcodeZoom = "medium";
                    break;
                case "L":
                    SmallTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    MediumTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    LargeTextBlock.Foreground = App.Current.Resources["StarGreenBrush"] as SolidColorBrush;
                    ViewModel.PreferredBarcodeZoom = "large";
                    break;
                default:
                    SmallTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    MediumTextBlock.Foreground = App.Current.Resources["StarGreenBrush"] as SolidColorBrush;
                    LargeTextBlock.Foreground = App.Current.Resources["StarTanBrush"] as SolidColorBrush;
                    ViewModel.PreferredBarcodeZoom = "medium";
                    break;

            }
        }

        private void EditButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CardEditView), ViewModel.SelectedCard);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is StarbucksCard card)
            {
                ViewModel.SelectedCard = card;
            }
        }
    }
}
