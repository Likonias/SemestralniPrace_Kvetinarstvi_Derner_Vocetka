using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
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
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateFlowersCommand { get; }
        public ICommand NavigateAddressCommand { get; }
        public ICommand NavigateCustomerCommand { get; }
        public ICommand NavigateEmployeeCommand { get; }
        public ICommand NavigateOtherGoodsCommand { get; }
        public ICommand NavigateOrderCommand { get; }
        public ICommand NavigateMainCommand { get; }
        public ICommand NavigateAddressTypeCommand { get; }
        public ICommand NavigateBillingCommand { get; }
        public ICommand NavigateDeliveryMethodCommand { get; }
        public ICommand NavigateDeliveryCommand { get; }
        public ICommand NavigateInPersonPickupCommand { get; }
        public ICommand NavigateInvoiceCommand { get; }
        public ICommand NavigateOccasionCommand { get; }
        public ICommand NavigateOrderStatusCommand { get; }
        public ICommand NavigateSystemCatalogCommand { get; }

        public ObservableCollection<string> ComboBoxItems { get; set; }

        private string selectedComboBoxItem;
        private ComboBoxTableNamesEnum selectedEnumValue;

        private readonly AccountStore accountStore;
        private OracleDbUtil dbUtil;

        public bool IsLoggedIn => accountStore.IsLoggedIn;
        public bool IsLoggedOut => accountStore.IsLoggedOut;

        public bool IsCurrentAdmin => accountStore.IsCurrentAdmin;
        public ICommand EmulateCommand { get; }
        public ObservableCollection<string> EmulateComboBoxItems { get; set; }
        private string selectedEmulateComboBoxItem;
        public string SelectedEmulateComboBoxItem
        {
            get => selectedEmulateComboBoxItem;
            set
            {
                selectedEmulateComboBoxItem = value;
                OnPropertyChanged(nameof(SelectedEmulateComboBoxItem));
            }
        }

        public NavigationBarViewModel(AccountStore accountStore, NavigationServiceManager navigationServiceManager)
        {
            LoginCommand = new NavigateCommand<LoginViewModel>(navigationServiceManager.GetNavigationService<LoginViewModel>());
            RegisterCommand = new NavigateCommand<CustomerFormViewModel>(navigationServiceManager.GetNavigationService<CustomerFormViewModel>());
            LogoutCommand = new RelayCommand(Logout);
            NavigateMainCommand = new NavigateCommand<MainViewModel>(navigationServiceManager.GetNavigationService<MainViewModel>());
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(navigationServiceManager.GetNavigationService<AccountViewModel>());
            NavigateFlowersCommand = new NavigateCommand<FlowersViewModel>(navigationServiceManager.GetNavigationService<FlowersViewModel>());
            NavigateAddressCommand = new NavigateCommand<AddressViewModel>(navigationServiceManager.GetNavigationService<AddressViewModel>());
            NavigateCustomerCommand = new NavigateCommand<CustomerViewModel>(navigationServiceManager.GetNavigationService<CustomerViewModel>());
            NavigateEmployeeCommand = new NavigateCommand<EmployeeViewModel>(navigationServiceManager.GetNavigationService<EmployeeViewModel>());
            NavigateOtherGoodsCommand = new NavigateCommand<OtherGoodsViewModel>(navigationServiceManager.GetNavigationService<OtherGoodsViewModel>());
            NavigateOrderCommand = new NavigateCommand<OrderViewModel>(navigationServiceManager.GetNavigationService<OrderViewModel>());
            NavigateAddressTypeCommand = new NavigateCommand<AddressTypeViewModel>(navigationServiceManager.GetNavigationService<AddressTypeViewModel>());
            NavigateBillingCommand = new NavigateCommand<BillingViewModel>(navigationServiceManager.GetNavigationService<BillingViewModel>());
            NavigateDeliveryMethodCommand = new NavigateCommand<DeliveryMethodViewModel>(navigationServiceManager.GetNavigationService<DeliveryMethodViewModel>());
            NavigateDeliveryCommand = new NavigateCommand<DeliveryViewModel>(navigationServiceManager.GetNavigationService<DeliveryViewModel>());
            NavigateInPersonPickupCommand = new NavigateCommand<InPersonPickupViewModel>(navigationServiceManager.GetNavigationService<InPersonPickupViewModel>());
            NavigateInvoiceCommand = new NavigateCommand<InvoiceViewModel>(navigationServiceManager.GetNavigationService<InvoiceViewModel>());
            NavigateOccasionCommand = new NavigateCommand<OccasionViewModel>(navigationServiceManager.GetNavigationService<OccasionViewModel>());
            NavigateOrderStatusCommand = new NavigateCommand<OrderStatusViewModel>(navigationServiceManager.GetNavigationService<OrderStatusViewModel>());
            NavigateSystemCatalogCommand = new NavigateCommand<SystemCatalogViewModel>(navigationServiceManager.GetNavigationService<SystemCatalogViewModel>());

            this.accountStore = accountStore;
            ComboBoxItems = new ObservableCollection<string>();
            PopulateComboBox();

            EmulateCommand = new RelayCommand(Emulate);
            EmulateComboBoxItems = new ObservableCollection<string>();
            PopulateEmulateComboBox();

            this.accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
            dbUtil = new OracleDbUtil();
        }

        private async void PopulateEmulateComboBox()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            EmployeeRepository employeeRepository = new EmployeeRepository();
            await customerRepository.GetAll();
            await employeeRepository.GetAll();
            foreach (Customer customer in customerRepository.Customers) { EmulateComboBoxItems.Add(customer.Email); }
            foreach(Employee employee in employeeRepository.Employees) { EmulateComboBoxItems.Add(employee.Email); }
        }

        private async void Emulate()
        {
            Account acc = await dbUtil.ExecuteGetAccountFunctionAsync("getuserbyemail", SelectedEmulateComboBoxItem);
            accountStore.CurrentAccount = acc;
            NavigateAccountCommand.Execute(null);
        }

        private void Logout()
        {
            accountStore.Logout();
            NavigateMainCommand.Execute(null);
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
            OnPropertyChanged(nameof(IsCurrentAdmin));
        }

        public string SelectedComboBoxItem
        {
            get => selectedComboBoxItem;
            set
            {
                selectedComboBoxItem = value;
                OnPropertyChanged(nameof(SelectedComboBoxItem));

                //This code checks the combobox description and matches its enum value, so the enum can be used in a switch for View selection
                foreach (ComboBoxTableNamesEnum val in Enum.GetValues(typeof(ComboBoxTableNamesEnum)))
                {
                    FieldInfo fieldInfo = val.GetType().GetField(val.ToString());
                    DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                    string description = (attributes.Length > 0) ? attributes[0].Description : val.ToString();

                    if (value == description)
                    {
                        selectedEnumValue = val;
                        break;
                    }
                }
                SelectedViewCommandComboBox();
            }
        }

        private void PopulateComboBox()
        {
            ComboBoxItems.Clear();
            IEnumerable<ComboBoxTableNamesEnum> allowedValues = GetAllowedComboBoxValues(accountStore.CurrentAccount?.EmployeePosition);

            //populating ComboBox with the ComboBoxTableNamesEnums descriptions 
            foreach (ComboBoxTableNamesEnum value in allowedValues)
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                string description = (attributes.Length > 0) ? attributes[0].Description : value.ToString();
                ComboBoxItems.Add(description);
            }
        }

        private IEnumerable<ComboBoxTableNamesEnum> GetAllowedComboBoxValues(EmployeePositionEnum? employeePosition)
        {
            
            List<ComboBoxTableNamesEnum> allowedValues = new List<ComboBoxTableNamesEnum>();

            //TODO Finish setting up priviledges
            switch (employeePosition)
            {
                case EmployeePositionEnum.ADMIN:
                    foreach (ComboBoxTableNamesEnum value in (ComboBoxTableNamesEnum[])Enum.GetValues(typeof(ComboBoxTableNamesEnum)))
                    {
                        allowedValues.Add(value);
                    }
                    break;
                case EmployeePositionEnum.MAJITEL:
                    allowedValues.Add(ComboBoxTableNamesEnum.Addresses);
                    break;
                case EmployeePositionEnum.PRODAVAC:
                    allowedValues.Add(ComboBoxTableNamesEnum.Customers);
                    break;
                default:
                    allowedValues.Add(ComboBoxTableNamesEnum.Flowers);
                    allowedValues.Add(ComboBoxTableNamesEnum.Addresses);
                    allowedValues.Add(ComboBoxTableNamesEnum.Employees);
                    allowedValues.Add(ComboBoxTableNamesEnum.OtherGoods);
                    allowedValues.Add(ComboBoxTableNamesEnum.DeliveryMethods);
                    break;
                    
            }

            return allowedValues;
        }

        private void SelectedViewCommandComboBox()
        {
            switch(selectedEnumValue)
            {
                case ComboBoxTableNamesEnum.Flowers:
                    NavigateFlowersCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Addresses:
                    NavigateAddressCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Customers:
                    NavigateCustomerCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Employees:
                    NavigateEmployeeCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Orders:
                    NavigateOrderCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.OtherGoods:
                    NavigateOtherGoodsCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.AddressTypes:
                    NavigateAddressTypeCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Billings:
                    NavigateBillingCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.DeliveryMethods:
                    NavigateDeliveryMethodCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Deliveries:
                    NavigateDeliveryCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.InPersonPickups:
                    NavigateInPersonPickupCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Invoices:
                    NavigateInvoiceCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.Occasions:
                    NavigateOccasionCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.OrderStatus:
                    NavigateOrderStatusCommand.Execute(null);
                    break;
                case ComboBoxTableNamesEnum.SystemCatalog:
                    NavigateSystemCatalogCommand.Execute(null);
                    break;
            }
        }

        public override void Dispose()
        {
            this.accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }

    }
}
