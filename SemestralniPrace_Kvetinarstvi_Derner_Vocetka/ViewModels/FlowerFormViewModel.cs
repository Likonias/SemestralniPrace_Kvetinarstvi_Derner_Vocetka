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

        public FlowerFormViewModel(AccountStore accountStore, INavigationService closeModalNavigationService, FlowerStore flowerStore, INavigationService? openFlowerViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            flower = flowerStore.Flower;
            this.flowerStore = flowerStore;
            this.accountStore = accountStore;
            this.openFlowerViewModel = openFlowerViewModel;
            if (flower != null) { InitializeFlower(); }
            FlowerStateComboBoxItems = new ObservableCollection<string>();
            PopulateFlowerStateComboBox();
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
                    flower = new Flower(0, Name, Price, Type, Warehouse, null, 0, FlowerState ?? FlowerStateEnum.A, Age);
                    await flowerRepository.Add(flower);
                }
                else
                {
                    flower = new Flower(flowerStore.Flower.IdGoods, Name, Price, Type, Warehouse, null, flowerStore.Flower.IdFlower, FlowerState ?? FlowerStateEnum.A, Age);
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
            return Name != null || Price != 0 || Type != '\0' || Warehouse != 0 || Type != null || _age < 0;
        }

        private void InitializeFlower()
        {
            _name = flower.Name;
            _price = flower.Price;
            _type = flower.Type;
            _warehouse = flower.Warehouse;
            _age = flower.Age;
            _type = flower.Type;
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

        private char _type;
        public char Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
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
