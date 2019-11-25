using LatteLocator.Core.Models;

namespace LatteLocator.ViewModels
{
    public class CardEditViewModel : ViewModelBase
    {
        private StarbucksCard card;

        public CardEditViewModel()
        {
            Card = new StarbucksCard { Title = "", AccountNumber = 1234123412341234, ScannedData = "Notes" };
        }

        public StarbucksCard Card
        {
            get => card;
            set => SetProperty(ref card, value);
        }
    }
}
