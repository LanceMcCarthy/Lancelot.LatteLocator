using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LatteLocator.Core.Models;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Navigation;
using LatteLocator.Helpers;

namespace LatteLocator.Views
{
    public sealed partial class CardEditView : Page
    {
        private bool isEdit;
        private bool isValidCardNumber;

        public CardEditView()
        {
            this.InitializeComponent();
            Loaded += CardEditView_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is StarbucksCard card)
            {
                ViewModel.Card = card;
                isEdit = true;
            }
        }

        private void CardEditView_Loaded(object sender, RoutedEventArgs e)
        {
            CardTitleTextBox.Text = ViewModel.Card.Title;
            AccountNumberTextBox.Text = $"{ViewModel.Card.AccountNumber:0000000000000000}";
            NotesTextBox.Text = ViewModel.Card.ScannedData;
        }
        
        private async void SaveCardButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!isValidCardNumber)
            {
                await new MessageDialog("You must enter 16 numbers only. No letters, spaces or dashes are allowed.", "Invalid Card Number").ShowAsync();
                return;
            }

            if (string.IsNullOrEmpty(CardTitleTextBox.Text))
            {
                await new MessageDialog("The nickname and number fields must be filled out in order to save a card", "Missing Nickname").ShowAsync();
                return;
            }

            ViewModel.Card.Title = CardTitleTextBox.Text;
            ViewModel.Card.ScannedData = NotesTextBox.Text;
            ViewModel.Card.AccountNumber = long.TryParse(AccountNumberTextBox.Text, out long cardNumber) ? cardNumber : 0000000000000000;

            if (isEdit)
            {
                if (await CancelIfTitleChangedAsync())
                    return;
                
                await CardStorageManager.UpdateCardAsync(ViewModel.Card);
            }
            else
            {
                var cards = await CardStorageManager.LoadCardsAsync();

                var isDuplicate = cards.Any(card => card.Title == CardTitleTextBox.Text);

                if (isDuplicate)
                {
                    await new MessageDialog("The nickname you have already exists in your card list, each card needs a unique nickname.", "Nickname Exists").ShowAsync();
                    return;
                }
                
                await CardStorageManager.AddCardAsync(ViewModel.Card);
            }
            
            if(Frame.CanGoBack)
                Frame.GoBack();
        }

        private void AccountNumberTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var value = AccountNumberTextBox.Text;

            //AccountNumberTextBox.ChangeValidationState(ValidationState.Validating, "Validating");

            if (!IsOnlyNumbers(value))
            {
                //AccountNumberTextBox.ChangeValidationState(ValidationState.Invalid, "!--Numbers Only--!");
                isValidCardNumber = false;
                return;
            }

            if (!IsCardNumberLengthValid(value))
            {
                //AccountNumberTextBox.ChangeValidationState(ValidationState.Invalid, "16 numbers needed");
                isValidCardNumber = false;
                return;
            }

            //AccountNumberTextBox.ChangeValidationState(ValidationState.Valid, "valid");

            isValidCardNumber = true;
        }

        private static bool IsCardNumberLengthValid(string cardNumber)
        {
            return cardNumber.Length == 16; 
        }

        //user is notified if tile is pinned and is given option to cancel before saving
        private async Task<bool> CancelIfTitleChangedAsync()
        {
            //if the name has not been changed, return true
            if (ViewModel.Card.Title == CardTitleTextBox.Text)
                return false;

            var param = "CardTitle=" + ViewModel.Card.Title;

            //var tileToFind = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(param));

            var tiles = await SecondaryTile.FindAllAsync();
            var tileToFind = tiles.FirstOrDefault(t => t.Arguments.Contains(param));

            if (tileToFind == null)
                return false; 
            
            var message = "Because you are changing the nickname of this card, it will be unpinned from the start screen. You can re-pin it immediately after saving the change.";

            await new MessageDialog(message).ShowAsync();
            
            await tileToFind.RequestDeleteAsync();

            return false;
        }

        private bool IsOnlyNumbers(string input)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(input);
        }
    }
}
