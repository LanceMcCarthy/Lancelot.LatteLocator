using System.Runtime.Serialization;
using LatteLocator.Core.Common;

namespace LatteLocator.ViewModels
{
    [DataContract]
    public class ViewModelBase : ObservableObject
    {
        private bool isBusy;
        private string isBusyMessage;
        
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
    }
}
