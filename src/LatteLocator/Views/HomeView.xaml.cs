using Windows.UI.Xaml.Controls;
using LatteLocator.Core.Models;

namespace LatteLocator.Views
{
    public sealed partial class HomeView : Page
    {
        public HomeView()
        {
            this.InitializeComponent();
            DataContext = App.ViewModel;
        }

        private void ListViewBase_OnItemClick(object sender, ItemClickEventArgs e)
        {
            App.ViewModel.SelectedStore = e.ClickedItem as Store;
            Frame.Navigate(typeof (StoreDetailsView));
        }
    }
}
