using Microsoft.Win32;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class FlowerFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        public ICommand SelectImageCommand { get; }
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> FlowerStateComboBoxItems { get; set; }

        private INavigationService closeNavSer;
        private Flower flower;
        private FlowerStore flowerStore;
        private INavigationService openFlowerViewModel;
        private string fileName;
        private string fileExtension;

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
            _image = null;
            SelectImageCommand = new RelayCommand(SelectImage);
        }

        private void PopulateFlowerStateComboBox()
        {
            FlowerStateComboBoxItems.Clear();

            foreach (FlowerStateEnum value in Enum.GetValues(typeof(FlowerStateEnum)))
            {
                FlowerStateComboBoxItems.Add(value.ToString());
            }
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
            if (CheckFlower())
            {
                FlowerRepository flowerRepository = new FlowerRepository();

                if (flowerStore.Flower == null)
                {
                    flower = new Flower(0, Name, Price, 'K', Warehouse, Image, 0, FlowerState ?? FlowerStateEnum.A, Age);
                    await flowerRepository.Add(flower, fileName, fileExtension);
                }
                else
                {
                    flower = new Flower(flowerStore.Flower.IdGoods, Name, Price, 'K', Warehouse, Image, flowerStore.Flower.IdFlower, FlowerState ?? FlowerStateEnum.A, Age);
                    await flowerRepository.Update(flower, fileName, fileExtension);
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
