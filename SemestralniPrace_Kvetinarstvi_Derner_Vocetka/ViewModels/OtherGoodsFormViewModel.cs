using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OtherGoodsFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        public ICommand SelectImageCommand { get; }
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> OtherGoodsTypeComboBoxItems { get; set; }

        private INavigationService closeNavSer;
        private OtherGoods otherGoods;
        private OtherGoodsStore otherGoodsStore;
        private INavigationService openOtherGoodsViewModel;
        private string fileName;
        private string fileExtension;

        public OtherGoodsFormViewModel(INavigationService closeModalNavigationService, OtherGoodsStore otherGoodsStore, INavigationService? openOtherGoodsViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            otherGoods = otherGoodsStore.OtherGoods;
            this.otherGoodsStore = otherGoodsStore;
            this.openOtherGoodsViewModel = openOtherGoodsViewModel;
            if (otherGoods != null) { InitializeOtherGoods(); }
            OtherGoodsTypeComboBoxItems = new ObservableCollection<string>();
            _image = null;
            SelectImageCommand = new RelayCommand(SelectImage);
        }
        private void SelectImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Title = "Select an Image";

            if (openFileDialog.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fileStream);
                string selectedImagePath = openFileDialog.FileName;
                fileName = Path.GetFileName(selectedImagePath);
                fileExtension = Path.GetExtension(selectedImagePath);
                //ALT File.readbytes...
                // Read the selected image into a byte array
                Image = br.ReadBytes((int)fileStream.Length);

                // Now 'imageData' contains the selected image as a byte array
                // You can use it as needed, such as storing it in your Flower model
                // flowerStore.Flower.ImageData = imageData; // Assuming Flower model has an ImageData property
            }
        }
        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckOtherGoods())
            {
                OtherGoodsRepository otherGoodsRepository = new OtherGoodsRepository();

                if (otherGoodsStore.OtherGoods == null)
                {
                    otherGoods = new OtherGoods(0, Name, Price, 'O', Warehouse, Image, 0, CountryOfOrigin, (DateOnly)ExpirationDate);
                    await otherGoodsRepository.Add(otherGoods, fileName, fileExtension);
                }
                else
                {
                    otherGoods = new OtherGoods(otherGoodsStore.OtherGoods.IdGoods, Name, Price, 'O', Warehouse, Image, otherGoodsStore.OtherGoods.IdOtherGoods, CountryOfOrigin, (DateOnly)ExpirationDate);
                    await otherGoodsRepository.Update(otherGoods, fileName, fileExtension);
                }

                closeNavSer.Navigate();
                openOtherGoodsViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckOtherGoods()
        {
            return Name != null || Price != 0 || Warehouse != 0 || CountryOfOrigin != null || ExpirationDate != null;
        }

        private void InitializeOtherGoods()
        {
            _name = otherGoods.Name;
            _price = otherGoods.Price;
            _warehouse = otherGoods.Warehouse;
            _countryOfOrigin = otherGoods.CountryOfOrigin;
            _expirationDate = otherGoods.ExpirationDate;
            _image = otherGoods.Image;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private byte[] _image;
        public byte[] Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged(nameof(Price));
            }
        }

        private int _warehouse;
        public int Warehouse
        {
            get => _warehouse;
            set
            {
                _warehouse = value;
                OnPropertyChanged(nameof(Warehouse));
            }
        }

        private string _countryOfOrigin;
        public string CountryOfOrigin
        {
            get => _countryOfOrigin;
            set
            {
                _countryOfOrigin = value;
                OnPropertyChanged(nameof(CountryOfOrigin));
            }
        }
        
        private DateOnly? _expirationDate;
        public DateOnly? ExpirationDate
        {
            get { return _expirationDate; }
            set
            {
                _expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        
        private DateTime selectedDate;
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                ExpirationDate = DateOnly.FromDateTime(selectedDate);
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

    }
}
