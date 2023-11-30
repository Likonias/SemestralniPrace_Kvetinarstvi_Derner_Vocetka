using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        
        private INavigationService closeNavSer;
        private Address address;
        public AddressFormViewModel(INavigationService closeModalNavigationService, AddressStore addressStore)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            address = addressStore.Address;
            if (address != null) { InitializeAddress(); }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private void Ok()
        {
            address = new Address(0, Street, StreetNumber, City, Zip, null, null, null);

            closeNavSer.Navigate();
        }

        //todo to populate comboboxes, the address owner is a list of all employees and all customers
        //or only all customers depending on the permissions, address type shows all avaiable address types based on addresstypeEnum

        private void InitializeAddress()
        {
            _street = address.Street;
            _streetNumber = address.StreetNumber;
            _city = address.City;
            _zip = address.Zip;
            _addressOwner = address.CustomerId.ToString();
            if(_addressOwner == null) { _addressOwner = address.EmployeeId.ToString(); }
            _addressType = address.AddressType.ToString();

        }

        private string _street;
        public string Street
        {
            get => _street;
            set
            {
                _street = value;
                OnPropertyChanged(nameof(Street));
            }
        }

        private string _streetNumber;
        public string StreetNumber
        {
            get => _streetNumber;
            set
            {
                _streetNumber = value;
                OnPropertyChanged(nameof(StreetNumber));
            }
        }

        private string _city;
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _zip;
        public string Zip
        {
            get => _zip;
            set
            {
                _zip = value;
                OnPropertyChanged(nameof(Zip));
            }
        }

        private string? _addressOwner;
        public string AddressOwner
        {
            get => _addressOwner;
            set
            {
                _addressOwner = value;
                OnPropertyChanged(nameof(AddressOwner));
            }
        }

        private string? _addressType;
        public string AddressType
        {
            get => _addressType;
            set
            {
                _addressType = value;
                OnPropertyChanged(nameof(AddressType));
            }
        }


    }
}
