using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public RelayCommand BtnOk { get; private set; }
        private readonly AccountStore accountStore;
        public string errorMessage;
        public ObservableCollection<string> AddressTypeComboBoxItems { get; set; }
        public ObservableCollection<string> AddressOwnerComboBoxItems { get; set; }


        private INavigationService closeNavSer;
        private Address address;
        private AddressStore addressStore;
        private INavigationService openAddressViewModel;
        private OracleDbUtil dbUtil;
        public AddressFormViewModel(AccountStore accountStore, INavigationService closeModalNavigationService, AddressStore addressStore, INavigationService openAddressViewModel)
        {
            closeNavSer = closeModalNavigationService;
            dbUtil = new OracleDbUtil();
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            address = addressStore.Address;
            this.addressStore = addressStore;
            this.accountStore = accountStore;
            this.openAddressViewModel = openAddressViewModel;
            if (address != null) { InitializeAddress(); }
            AddressOwnerComboBoxItems = new ObservableCollection<string>();
            AddressTypeComboBoxItems = new ObservableCollection<string>();
            PopulateAddressTypeComboBox();
            PopulateAddressOwnerComboBox();
        }

        private void PopulateAddressTypeComboBox()
        {
            AddressTypeComboBoxItems.Clear();

            foreach (AddressTypeEnum value in Enum.GetValues(typeof(AddressTypeEnum)))
            {
                AddressTypeComboBoxItems.Add(value.ToString());
            }
        }

        private void PopulateAddressOwnerComboBox()
        {
            AddressOwnerComboBoxItems.Clear();
            IEnumerable<string> allowedValues = GetAllowedAddressOwners(accountStore.CurrentAccount?.EmployeePosition);

            // Populating ComboBox with the allowed address owners
            foreach (string value in allowedValues)
            {
                AddressOwnerComboBoxItems.Add(value);
            }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckAddress())
            {
                //todo finish setting up id if it is a customer or a employee logic a taky address typ id
                AddressRepository addressRepository = new AddressRepository();

                Account selectedAcc = await dbUtil.ExecuteGetAccountFunctionAsync("getUserByEmail", AddressOwner);

                if (addressStore.Address == null)
                {
                    if(selectedAcc.EmployeePosition == null)
                    {
                        address = new Address(0, Street, StreetNumber, City, Zip, null, selectedAcc.Id, AddressTypeComboBoxItems.IndexOf(AddressType) + 1);
                    }
                    else
                    {
                        address = new Address(0, Street, StreetNumber, City, Zip, selectedAcc.Id, null, AddressTypeComboBoxItems.IndexOf(AddressType) + 1);
                    }
                    
                    await addressRepository.Add(address);
                }
                else 
                {
                    if (selectedAcc.EmployeePosition == null)
                    {
                        address = new Address(addressStore.Address.Id, Street, StreetNumber, City, Zip, null, selectedAcc.Id, AddressTypeComboBoxItems.IndexOf(AddressType) + 1);
                    }
                    else
                    {
                        address = new Address(addressStore.Address.Id, Street, StreetNumber, City, Zip, selectedAcc.Id, null, AddressTypeComboBoxItems.IndexOf(AddressType) + 1);
                    }
                    await addressRepository.Update(address);
                }
                
                closeNavSer.Navigate();
                openAddressViewModel.Navigate();
            }
            else 
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckAddress()
        {
            return Street != null || StreetNumber != null || City != null || Zip != null || AddressOwner != null || AddressType != null;
        }

        private void InitializeAddress()
        {
            _street = address.Street;
            _streetNumber = address.StreetNumber;
            _city = address.City;
            _zip = address.Zip;
            _addressOwner = address.CustomerId.ToString();
            if(_addressOwner == null) { _addressOwner = address.EmployeeId.ToString(); }
            _addressType = address.AddressTypeId.ToString();
        }

        private List<string> GetAllowedAddressOwners(EmployeePositionEnum? employeePosition)
        {
            List<string> allowedValues = new List<string>();

            CustomerRepository customerRepository = new CustomerRepository();
            EmployeeRepository employeeRepository = new EmployeeRepository();

            string currentEmail = accountStore.CurrentAccount?.Email;

            switch (employeePosition)
            {
                case EmployeePositionEnum.ADMIN:
                    allowedValues.AddRange(customerRepository.GetCustomers().Select(c => c.Email));
                    allowedValues.AddRange(employeeRepository.GetEmployees().Select(e => e.Email));
                    break;
                case EmployeePositionEnum.MAJITEL:
                    allowedValues.AddRange(customerRepository.GetCustomers().Select(c => c.Email));
                    allowedValues.AddRange(employeeRepository.GetEmployees().Select(e => e.Email));
                    break;
                case EmployeePositionEnum.PRODAVAC:
                    allowedValues.Add(currentEmail);
                    break;
                default:
                    break;
            }

            return allowedValues;
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
