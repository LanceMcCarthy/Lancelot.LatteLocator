using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Store;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.UI.Popups;
using LatteLocator.Core.Common;
using LatteLocator.Core.Models;
using LatteLocator.Core.Services;
using System.Runtime.Serialization;

namespace LatteLocator.ViewModels
{
    [DataContract]
    public class MainViewModel : ViewModelBase
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private readonly StarbucksApi api;
        private readonly Geolocator geolocator;
        private StarbucksSearchResult searchResult;
        private ObservableCollection<StarbucksCard> starbucksCards;
        private List<string> barcodeZooms;
        private DelegateCommand refreshStoresCommand;
        
        private Store selectedStore;
        private StarbucksCard selectedStarbucksCard;
        private Geoposition myLocation;
        private bool isAppLocked;
        private bool isDirty;
        
        private bool? useMetric = false;
        private bool areAdsRemoved;

        public MainViewModel()
        {
            api = new StarbucksApi();

            if (!DesignMode.DesignModeEnabled)
            {
                geolocator = new Geolocator();
            }
            
            //RefreshStoresCommand.Execute(null);
        }

        #region collections

        [DataMember]
        public StarbucksSearchResult SearchResult
        {
            get => searchResult;
            set => SetProperty(ref searchResult, value);
        }
        
        public List<string> BarcodeZooms => barcodeZooms ?? (barcodeZooms = new List<string> { "small", "medium", "large" });

        #endregion

        #region commands

        public DelegateCommand RefreshStoresCommand
        {
            get
            {
                return refreshStoresCommand ?? (refreshStoresCommand = new DelegateCommand(async () =>
                {
                    MyLocation = await GetCurrentLocationAsync();

                    await FindLocationsAsync(
                        MyLocation.Coordinate.Point.Position.Longitude, 
                        MyLocation.Coordinate.Point.Position.Latitude);
                }));
            }
            set => refreshStoresCommand = value;
        }
        

        #endregion

        #region runtime properties
        
        [DataMember]
        public bool IsDirty
        {
            get => isDirty;
            set => SetProperty(ref isDirty, value);
        }

        public bool IsConnectedToNetwork => NetworkInterface.GetIsNetworkAvailable();

        [DataMember]
        public Store SelectedStore
        {
            get => selectedStore;
            set => SetProperty(ref selectedStore, value);
        }

        //[DataMember]
        //public StarbucksCard SelectedStarbucksCard
        //{
        //    get => selectedStarbucksCard;
        //    set => SetProperty(ref selectedStarbucksCard, value);
        //}

        [DataMember]
        public Geoposition MyLocation
        {
            get
            {
                if (DesignMode.DesignModeEnabled)
                {
                    //Latitude = 42.3845, Longitude = -071.2365 
                    return myLocation;
                }

                return myLocation ?? (myLocation = GetCurrentLocationAsync().Result);
            }
            set => SetProperty(ref myLocation, value);
        }
        
        [DataMember]
        public bool AreAdsRemoved
        {
            get
            {
                //return true;
                areAdsRemoved = CurrentApp.LicenseInformation.ProductLicenses["RemoveAds"].IsActive;
                Debug.WriteLine("AreAdsRemoved {0}", areAdsRemoved);
                return areAdsRemoved;
            }
            set => SetProperty(ref areAdsRemoved, value);
        }

        public string AppVersion => "10.0";

        #endregion

        #region Persisted Properties
        
        public bool LocationEnabled
        {
            get
            {
                object val;

                if (localSettings.Values.TryGetValue("LocationEnabled", out val))
                {
                    val = (bool)localSettings.Values["LocationEnabled"];
                }
                else
                {
                    return true;
                }

                Debug.WriteLine("LocationEnabled VMSetting {0}", val);

                return (bool)val;
            }
            set
            {
                localSettings.Values["LocationEnabled"] = value;
                OnPropertyChanged();
            }
        }
        
        public bool? UseMetric
        {
            get
            {
                object obj;
                if (localSettings.Values.TryGetValue("UseMetric", out obj))
                {
                    useMetric = (bool?)obj;
                }

                return useMetric;
            }
            set
            {
                if (value == useMetric)
                    return;
                useMetric = value;
                localSettings.Values["UseMetric"] = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsAppLocked
        {
            get
            {
                object obj;
                if (localSettings.Values.TryGetValue("IsAppLocked", out obj))
                {
                    isAppLocked = (bool)obj;
                }

                Debug.WriteLine("Is App Locked {0}", isAppLocked);

                return isAppLocked;
            }
            set
            {
                if (isAppLocked == value) return;
                isAppLocked = value;
                localSettings.Values["IsAppLocked"] = value;
                OnPropertyChanged();
            }
        }


        #endregion

        #region events
        
        public async Task<Geoposition> GetCurrentLocationAsync()
        {
            try
            {
                IsBusy = true;
                IsBusyMessage = "getting your location...";

                geolocator.DesiredAccuracy = PositionAccuracy.Default;
                geolocator.DesiredAccuracyInMeters = 100;

                var pos = await geolocator.GetGeopositionAsync();
                
                return pos;
            }
            catch (UnauthorizedAccessException ex)
            {

            }
            catch (TaskCanceledException ex)
            {

            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusyMessage = "";
                IsBusy = false;
            }

            return null;
        }

        public async Task FindLocationsAsync(double longitude, double latitude)
        {
            if (!IsConnectedToNetwork)
            {
                await new MessageDialog("This application requires a data connection to function. Refresh when you are connected.").ShowAsync();
                return;
            }

            IsBusy = true;
            IsBusyMessage = "finding nearby Starbucks...";

            try
            {
                var result = await api.SearchNearbyStores(longitude, latitude);

                if (result != null)
                    SearchResult = result;
                
            }
            catch (Exception ex)
            {
                //App.TelemetryClient.TrackEvent("Location Fetch Failed");

                await new MessageDialog("There was an error from the server. \r\n\nError:"
                                        + ex.Message
                                        + "\r\n\nThere may not be any stores in your area, go to the settings page and increase the search radius. \r\n\nThen come back here and pull to refresh.").ShowAsync();
            }
            finally
            {
                IsBusy = false;
                IsBusyMessage = "";
            }
        }
        
        

        public async void PurchaseAdUnlock()
        {
            try
            {
                 var result = await CurrentApp.RequestProductPurchaseAsync("RemoveAds");

                if (result.Status == ProductPurchaseStatus.Succeeded ||
                    result.Status == ProductPurchaseStatus.AlreadyPurchased)
                {
                    AreAdsRemoved = true;
                }


                //MessageBox.Show("Thank you for your purchase. Enjoy your java searches without ads! r\r\nIf the ad does not hide immediately, closing and relaunching the app will hide them.", "Purchase Complete!", MessageBoxButton.OK);
                //var forceRefresher = AreAdsRemoved; //just to force a Get on the IsActive value of the IAP
                //App.TelemetryClient.TrackEvent("IAP Purchase Successful");
            }
            catch
            {
                //var answer = MessageBox.Show("Something went wrong and your account was not charged. Would you like to try again?", "Incomplete Transaction", MessageBoxButton.OKCancel);

                //if(answer == MessageBoxResult.OK)
                //{
                //    PurchaseAdUnlock();
                //}
                //else
                //{
                //    App.TelemetryClient.TrackEvent("IAP Purchase Unsuccessful");
                //}
            }
        }
        
        #endregion
    }
}
