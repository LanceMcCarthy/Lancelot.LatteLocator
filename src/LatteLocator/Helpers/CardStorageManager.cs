using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.DataProtection;
using Windows.Storage;
using Windows.Storage.Streams;
using LatteLocator.Core.Models;
using Newtonsoft.Json;

namespace LatteLocator.Helpers
{
    public static class CardStorageManager
    {
        private static readonly ApplicationDataContainer RoamingSettings;

        static CardStorageManager()
        {
            if (!DesignMode.DesignModeEnabled || !DesignMode.DesignMode2Enabled)
            {
                RoamingSettings = ApplicationData.Current.RoamingSettings;
            }
        }
        
        public static async Task<ObservableCollection<StarbucksCard>> LoadCardsAsync()
        {
            var decryptedCards = new ObservableCollection<StarbucksCard>();

            if (!RoamingSettings.Values.ContainsKey("StarbucksCards"))
            {
                return decryptedCards;
            }

            if (RoamingSettings.Values.TryGetValue("StarbucksCards", out var storedValue))
            {
                var protectedCards = (ObservableCollection<StarbucksCard>)storedValue;

                foreach (var protectedCard in protectedCards)
                {
                    var decryptedCard = new StarbucksCard
                    {
                        Title = protectedCard.Title
                    };

                    // Decrypt account/card number
                    IBuffer protectedAcctNumberBuffer = System.Text.Encoding.UTF8.GetBytes(protectedCard.AccountNumber.ToString()).AsBuffer();
                    var unprotectedAcctNumberBuffer = await new DataProtectionProvider().UnprotectAsync(protectedAcctNumberBuffer);
                    var acctNumClearText = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, unprotectedAcctNumberBuffer);
                    decryptedCard.AccountNumber = Convert.ToInt64(acctNumClearText);

                    // Decrypt scan data
                    IBuffer protectedScanDataBuffer = System.Text.Encoding.UTF8.GetBytes(protectedCard.ScannedData).AsBuffer();
                    var unprotectedScanDataBuffer = await new DataProtectionProvider().UnprotectAsync(protectedScanDataBuffer);
                    var scanDataClearText = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, unprotectedScanDataBuffer);
                    decryptedCard.ScannedData = scanDataClearText;

                    // Add the card to runtime list
                    decryptedCards.Add(decryptedCard);
                }
            }

            return decryptedCards;
        }

        public static async Task SaveCards(ObservableCollection<StarbucksCard> unprotectedCards)
        {
            var protectedCards = new ObservableCollection<StarbucksCard>();

            foreach (var unprotectedCard in unprotectedCards)
            {
                var protectedCard = new StarbucksCard
                {
                    Title = unprotectedCard.Title
                };

                // encrypt card number at rest
                var numberData = CryptographicBuffer.ConvertStringToBinary(unprotectedCard.AccountNumber.ToString(), BinaryStringEncoding.Utf8);
                var numberDataProtected = await new DataProtectionProvider().ProtectAsync(numberData);
                protectedCard.AccountNumber = Convert.ToInt64(numberDataProtected);

                // encrypt scanned data at rest
                var scanData = CryptographicBuffer.ConvertStringToBinary(unprotectedCard.ScannedData, BinaryStringEncoding.Utf8);
                var scanDataProtected = await new DataProtectionProvider().ProtectAsync(scanData);
                protectedCard.ScannedData = Convert.ToString(scanDataProtected);
            }

            RoamingSettings.Values["StarbucksCards"] = JsonConvert.SerializeObject(protectedCards);
        }

        public static async Task AddCardAsync(StarbucksCard card)
        {
            var cards = await LoadCardsAsync();

            if (cards.Contains(card))
                return;

            cards.Add(card);

            await SaveCards(cards);
        }

        public static async Task RemoveCardAsync(StarbucksCard card)
        {
            var cards = await LoadCardsAsync();

            if (!cards.Contains(card))
                return;

            cards.Remove(card);

            await SaveCards(cards);
        }

        public static async Task UpdateCardAsync(StarbucksCard card)
        {
            var cards = await LoadCardsAsync();
            var cardToUpdate = cards.FirstOrDefault(x => x == card);

            if (cardToUpdate == null)
                return;

            cardToUpdate.AccountNumber = card.AccountNumber;
            cardToUpdate.ScannedData = card.ScannedData;
            cardToUpdate.Title = card.Title;

            await SaveCards(cards);
        }
    }
}
