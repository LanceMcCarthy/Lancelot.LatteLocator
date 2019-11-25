using System.Runtime.Serialization;
using LatteLocator.SharedCore.Helpers;

namespace LatteLocator.SharedCore.ViewModels
{
    [DataContract]
    public class ViewModelBase : ObservableObject
    {
        private bool isBusy;
        private string isBusyMessage;
        string title;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public string IsBusyMessage
        {
            get => isBusyMessage;
            set => SetProperty(ref isBusyMessage, value);
        }

        
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
    }
}
