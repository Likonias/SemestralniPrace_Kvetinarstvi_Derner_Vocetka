using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class FlowerFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> FlowerStateComboBoxItems { get; set; }

        private INavigationService closeNavSer;
        private Flower flower;
        private FlowerStore flowerStore;
        private INavigationService openFlowerViewModel;

        public FlowerFormViewModel(INavigationService closeModalNavigationService, FlowerStore flowerStore, INavigationService? openFlowerViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            flower = flowerStore.Flower;
            this.flowerStore = flowerStore;
            this.openFlowerViewModel = openFlowerViewModel;
            if (flower != null) { InitializeFlower(); }
            FlowerStateComboBoxItems = new ObservableCollection<string>();
            PopulateFlowerStateComboBox();
            _image = new byte[16];
        }

        private void PopulateFlowerStateComboBox()
        {
            FlowerStateComboBoxItems.Clear();

            foreach (FlowerStateEnum value in Enum.GetValues(typeof(FlowerStateEnum)))
            {
                FlowerStateComboBoxItems.Add(value.ToString());
            }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckFlower())
            {
                FlowerRepository flowerRepository = new FlowerRepository();

                if (flowerStore.Flower == null)
                {
                    flower = new Flower(0, Name, Price, 'K', Warehouse, Image, 0, FlowerState ?? FlowerStateEnum.A, Age);
                    await flowerRepository.Add(flower);
                }
                else
                {
                    flower = new Flower(flowerStore.Flower.IdGoods, Name, Price, 'K', Warehouse, Image, flowerStore.Flower.IdFlower, FlowerState ?? FlowerStateEnum.A, Age);
                    await flowerRepository.Update(flower);
                }

                closeNavSer.Navigate();
                openFlowerViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckFlower()
        {
            return Name != null || Price != 0 || Warehouse != 0 || _age < 0;
        }

        private void InitializeFlower()
        {
            _name = flower.Name;
            _price = flower.Price;
            _warehouse = flower.Warehouse;
            _age = flower.Age;
            _flowerState = flower.State;
            _image = flower.Image;
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

        private FlowerStateEnum? _flowerState;
        public FlowerStateEnum? FlowerState
        {
            get { return _flowerState; }
            set
            {
                _flowerState = value;
                OnPropertyChanged(nameof(FlowerState));
            }
        }


        private int _age;
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                OnPropertyChanged(nameof(Age));
            }
        }
    }
}
