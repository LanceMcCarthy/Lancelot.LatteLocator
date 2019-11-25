using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;

namespace LatteLocator.Views
{
    public sealed partial class StoreDetailsView : Page
    {
        public StoreDetailsView()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
            Loaded += StoreDetailsView_Loaded;
        }

        private async void StoreDetailsView_Loaded(object sender, RoutedEventArgs e)
        {
            var storePosition = new Geopoint(new BasicGeoposition
            {
                Latitude = App.ViewModel.SelectedStore.Coordinates.Latitude,
                Longitude = App.ViewModel.SelectedStore.Coordinates.Longitude
            });

            map.Center = storePosition;
            
            map.MapElements.Add(new MapIcon
            {
                Location = storePosition,
                NormalizedAnchorPoint = new Point(0.5, 1.0),
                Title = App.ViewModel.SelectedStore.Name,
                CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                Image = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/MapPin50.png"))
            });
        }

        private void Map_OnLoaded(object sender, RoutedEventArgs e)
        {
            //await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    var storePosition = new Geopoint(new BasicGeoposition
            //    {
            //        Latitude = App.ViewModel.SelectedStore.store.coordinates.latitude,
            //        Longitude = App.ViewModel.SelectedStore.store.coordinates.longitude
            //    });

            //    map.Center = storePosition;

            //    map.Children.Add(new MapIcon()
            //    {
            //        Location = storePosition,
            //        Title = App.ViewModel.SelectedStore.store.name,
            //        Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Images/MapPin40.png"))
            //    });

            //    //var thisPin = new Pushpin
            //    //{
            //    //    Location = coordinate,
            //    //    ContentTemplate = this.Resources["PinTemplate"] as DataTemplate
            //    //};

            //    //if (thisPin.Location != null)
            //    //    map.Children.Add(thisPin);
            //});

            

            //BusyIndicator.IsActive = false;
            //BusyIndicator.Visibility = Visibility.Collapsed;

        }

        private async void ShowStreetsideView()
        {
            // Check if Streetside is supported
            if (map.IsStreetsideSupported)
            {
                // Find panorama near 
                //var storePosition = new Geopoint(new BasicGeoposition
                //{
                //    Latitude = App.ViewModel.SelectedStore.store.coordinates.latitude,
                //    Longitude = App.ViewModel.SelectedStore.store.coordinates.longitude
                //});

                var storePosition = new Geopoint(new BasicGeoposition
                {
                    Latitude = App.ViewModel.SelectedStore.Coordinates.Latitude,
                    Longitude = App.ViewModel.SelectedStore.Coordinates.Longitude
                });

                var panoramaNearCity = await StreetsidePanorama.FindNearbyAsync(storePosition);

                // Set Streetside view if panorama exists
                if (panoramaNearCity != null)
                {
                    // Create Streetside view
                    StreetsideExperience ssView = new StreetsideExperience(panoramaNearCity);
                    ssView.OverviewMapVisible = true;
                    map.CustomExperience = ssView;
                }
            }
            else
            {
                // If Streetside is not supported
                ContentDialog viewNotSupportedDialog = new ContentDialog()
                {
                    Title = "Streetside is not supported",
                    Content = "\nStreetside views are not supported on this device.",
                    PrimaryButtonText = "OK"
                };
                await viewNotSupportedDialog.ShowAsync();
            }
        }

        private void map_MapResolved(object sender, EventArgs e)
        {
            

            //ZoomInButton.IsEnabled = true;
            //ZoomOutButton.IsEnabled = true;
        }
    }
}
