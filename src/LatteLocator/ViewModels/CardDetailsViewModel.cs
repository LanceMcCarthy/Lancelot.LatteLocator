using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using LatteLocator.Core.Models;
using ZXing;
using ZXing.PDF417;
using ZXing.PDF417.Internal;
using Windows.Foundation;
using Windows.Storage;

namespace LatteLocator.ViewModels
{
    public class CardDetailsViewModel : ViewModelBase
    {
        private readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private StarbucksCard selectedCard;
        private WriteableBitmap barcodeImage;
        private int barcodeErrorCorrectionLevel;
        private string preferredZoom = "medium";
        private Size preferredBarcodeSize = Size.Empty;
        private bool? alwaysUseFullScreenMode = false;

        public CardDetailsViewModel()
        {
            GenerateBarcode();
        }

        public StarbucksCard SelectedCard
        {
            get => selectedCard;
            set
            {
                SetProperty(ref selectedCard, value);
                GenerateBarcode();
            }
        }

        public WriteableBitmap BarcodeImage
        {
            get => barcodeImage;
            set => SetProperty(ref barcodeImage, value);
        }

        public bool IsAppLocked => App.ViewModel.IsAppLocked;

        public List<string> EccModes
        {
            get
            {
                return Enum.GetValues(typeof(PDF417ErrorCorrectionLevel)).Cast<Enum>().Select(x => x.ToString()).ToList();
            }
        }
        
        public int BarcodeErrorCorrectionLevel
        {
            get
            {
                object obj;
                if (localSettings.Values.TryGetValue("BarcodeErrorCorrectionLevel", out obj))
                {
                    barcodeErrorCorrectionLevel = (int)obj;
                }
                return barcodeErrorCorrectionLevel;
            }
            set
            {
                if (barcodeErrorCorrectionLevel == value) return;
                localSettings.Values["BarcodeErrorCorrectionLevel"] = value;
                barcodeErrorCorrectionLevel = value;
                OnPropertyChanged();
            }
        }
        
        public string PreferredBarcodeZoom
        {
            get
            {
                object obj;
                if (localSettings.Values.TryGetValue("PreferredBarcodeZoom", out obj))
                {
                    preferredZoom = (string)obj;
                }

                return preferredZoom;
            }
            set
            {
                if (preferredZoom == value) return;
                localSettings.Values["PreferredBarcodeZoom"] = value;
                preferredZoom = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(PreferredBarcodeSize));

                ////force prop change on the Size, too
                //switch (value)
                //{
                //    case "small":
                //        PreferredBarcodeSize = new Size(380, 80);
                //        break;
                //    case "medium":
                //        PreferredBarcodeSize = new Size(420, 90);
                //        break;
                //    case "large":
                //        PreferredBarcodeSize = new Size(460, 100);
                //        break;
                //    default:
                //        PreferredBarcodeSize = new Size(420, 90);
                //        break;
                //}
            }
        }
        
        public Size PreferredBarcodeSize
        {
            get
            {
                switch (PreferredBarcodeZoom)
                {
                    case "small":
                        preferredBarcodeSize = new Size(380, 80);
                        break;
                    case "medium":
                        preferredBarcodeSize = new Size(420, 90);
                        break;
                    case "large":
                        preferredBarcodeSize = new Size(460, 100);
                        break;
                    default:
                        preferredBarcodeSize = new Size(420, 90);
                        break;
                }

                return preferredBarcodeSize;
            }
            set
            {
                if (preferredBarcodeSize == value) return;
                preferredBarcodeSize = value;
                OnPropertyChanged();
            }
        }

        public bool? AlwaysUseFullScreenMode
        {
            get
            {
                object obj;
                if (localSettings.Values.TryGetValue("AlwaysUseFullScreenMode", out obj))
                {
                    alwaysUseFullScreenMode = (bool?)obj;
                }

                return alwaysUseFullScreenMode;
            }
            set
            {
                if (value == alwaysUseFullScreenMode)
                    return;
                alwaysUseFullScreenMode = value;
                localSettings.Values["AlwaysUseFullScreenMode"] = value;
                OnPropertyChanged();
            }
        }

        private void GenerateBarcode()
        {
            try
            {
                IsBusy = true;
                IsBusyMessage = "generating barcode...";

                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.PDF_417,
                    Options = new PDF417EncodingOptions
                    {
                        Height = 200,
                        Width = 200,
                        Margin = 10,
                        ErrorCorrection = (PDF417ErrorCorrectionLevel) BarcodeErrorCorrectionLevel,
                        CharacterSet = "Byte"
                    }
                };

                BarcodeImage = writer.Write(SelectedCard.AccountNumber.ToString());
            }
            catch (Exception e)
            {
                Debug.WriteLine($"GenerateBarcode Exception: {e}");
            }
            finally
            {
                IsBusy = false;
                IsBusyMessage = "";
            }
        }
    }
}
