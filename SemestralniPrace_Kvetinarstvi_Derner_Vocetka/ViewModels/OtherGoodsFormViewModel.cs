using System;
using System.Collections.ObjectModel;
using System.Windows;
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
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> OtherGoodsTypeComboBoxItems { get; set; }

        private INavigationService closeNavSer;
        private OtherGoods otherGoods;
        private OtherGoodsStore otherGoodsStore;
        private INavigationService openOtherGoodsViewModel;

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
            _image = new byte[16];
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
                    otherGoods = new OtherGoods(0, Name, Price, 'O', Warehouse, Image, 0, CountryOfOrigin, ExpirationDate ?? DateOnly.MinValue);
                    await otherGoodsRepository.Add(otherGoods);
                }
                else
                {
                    otherGoods = new OtherGoods(otherGoodsStore.OtherGoods.IdGoods, Name, Price, 'O', Warehouse, Image, otherGoodsStore.OtherGoods.IdOtherGoods, CountryOfOrigin, ExpirationDate ?? DateOnly.MinValue);
                    await otherGoodsRepository.Update(otherGoods);
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
    }
}
