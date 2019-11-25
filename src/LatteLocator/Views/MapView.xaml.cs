using System;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using LatteLocator.Core.Common;

namespace LatteLocator.Views
{
    public sealed partial class MapView : Page
    {
        public MapView()
        {
            this.InitializeComponent();

            // Make sure you have a MapServiceToken, otherwise I throw an exception
            map.MapServiceToken = Constants.GetMapServiceToken();

            DataContext = App.ViewModel;
            Loaded += MapView_Loaded;
        }

        private async void MapView_Loaded(object sender, RoutedEventArgs e)
        {
            var userPosition = new Geopoint(new BasicGeoposition
            {
                Latitude = App.ViewModel.MyLocation.Coordinate.Point.Position.Latitude,
                Longitude = App.ViewModel.MyLocation.Coordinate.Point.Position.Longitude
            });

            map.Center = userPosition;

            for (int i = 0; i < App.ViewModel.SearchResult.Stores.Count - 1; i++)
            {
                var store = App.ViewModel.SearchResult.Stores[i];

                var storePosition = new Geopoint(new BasicGeoposition
                {
                    Latitude = store.Coordinates.Latitude,
                    Longitude = store.Coordinates.Longitude,
                });

                var mapIcon = new MapIcon
                {
                    Location = storePosition,
                    NormalizedAnchorPoint = new Point(0.5, 1.0),
                    Title = store.Name,
                    CollisionBehaviorDesired = MapElementCollisionBehavior.RemainVisible,
                    ZIndex = i,
                    Image = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Images/MapPin40.png"))
                };


                map.MapElements.Add(mapIcon);
            }
        }
    }
}
