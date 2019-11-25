using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using LatteLocator.Core.Common;
using LatteLocator.Core.Models;
using LatteLocator.Helpers;

namespace LatteLocator.ViewModels
{
    public class CardsViewModel : ViewModelBase
    {
        private readonly ApplicationDataContainer localSettings;
        private StarbucksCard selectedCard;

        public CardsViewModel()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
            {
                StarbucksCards.Add(new StarbucksCard { Title = "Gold Card", AccountNumber = 1234567812345678, ScannedData = "Notes about the card would be here." });
                StarbucksCards.Add(new StarbucksCard { Title = "Gift Card", AccountNumber = 8765432187654321, ScannedData = "Notes about the card would be here." });
                return;
            }

            localSettings = ApplicationData.Current.LocalSettings;

            LoadCardsCommand = new DelegateCommand(async () => await LoadCardsAsync());
        }

        public ObservableCollection<StarbucksCard> StarbucksCards { get; } = new ObservableCollection<StarbucksCard>();

        public StarbucksCard SelectedCard
        {
            get => selectedCard ?? (selectedCard = new StarbucksCard());
            set => SetProperty(ref selectedCard, value);
        }

        public DelegateCommand LoadCardsCommand { get; set; }

        public bool IsAppLocked => App.ViewModel.IsAppLocked;

        private string Password
        {
            get
            {
                if (localSettings.Values.TryGetValue("Password", out object obj))
                {
                    Debug.WriteLine("Password {0}", obj);
                    return (string) obj;
                }

                Debug.WriteLine("Password Empty");
                return string.Empty;
            }
            set
            {
                localSettings.Values["Password"] = value;
                OnPropertyChanged();
            }
        }

        public bool SavePassword(string pass)
        {
            try
            {
                Password = pass;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckPassword(string pass)
        {
            try
            {
                return Password == pass;
            }
            catch
            {
                return false;
            }
        }

        public bool RemovePassword(string key)
        {
            Password = string.Empty;
            return true;
        }
        
        private async Task LoadCardsAsync()
        {
            if (await MigrateCardDataAsync())
            {
                var savedCards = await CardStorageManager.LoadCardsAsync();

                foreach (var savedCard in savedCards)
                {
                    StarbucksCards.Add(savedCard);
                }
            }
        }

        public async void AddCard(StarbucksCard card)
        {
            if (StarbucksCards.Contains(card))
                return;

            StarbucksCards.Add(card);

            await CardStorageManager.SaveCards(StarbucksCards);

            //App.TelemetryClient.TrackEvent("Card Added");
        }

        public async void RemoveCard(StarbucksCard card)
        {
            if (!StarbucksCards.Contains(card))
                return;

            StarbucksCards.Remove(card);

            await CardStorageManager.SaveCards(StarbucksCards);

            //App.TelemetryClient.TrackEvent("Card Deleted");

            //if tile is pinned, delete it
            //var param = "CardTitle=" + card.Title;
            //var tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(param));
            //if(tileToFind != null) tileToFind.Delete();
        }

        public async void UpdateCard(StarbucksCard card)
        {
            var cardToUpdate = StarbucksCards.FirstOrDefault(x => x == card);

            if (cardToUpdate == null)
                return;

            cardToUpdate.AccountNumber = card.AccountNumber;
            cardToUpdate.ScannedData = card.ScannedData;
            cardToUpdate.Title = card.Title;

            await CardStorageManager.SaveCards(StarbucksCards);
        }

        private async Task<bool> MigrateCardDataAsync()
        {
            //bool successfulMigration = false;

            try
            {
                // if setting is found and is true, then back out now
                if (localSettings.Values.TryGetValue("SuccessfulCardMigration", out object obj))
                {
                    if((bool)obj)
                        return true;
                }
                
                using (var fileStream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("__ApplicationSettings"))
                {
                    var knownTypes = new List<Type> { typeof(ObservableCollection<StarbucksCard>) };

                    var serializer =
                        new DataContractSerializer(typeof(ObservableCollection<StarbucksCard>), knownTypes);

                    var foundCards = (ObservableCollection<StarbucksCard>)serializer.ReadObject(fileStream);

                    Debug.WriteLine($"{foundCards.Count} cards discovered in old settings file");

                    foreach (var starbucksCard in foundCards)
                    {
                        StarbucksCards.Add(starbucksCard);
                    }

                    await CardStorageManager.SaveCards(StarbucksCards);

                    ApplicationData.Current.LocalSettings.Values["SuccessfulCardMigration"] = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MigrateCardData Exception: {ex}");
                //App.TelemetryClient.TrackException(ex);
                return false;
            }
        }
    }
}
