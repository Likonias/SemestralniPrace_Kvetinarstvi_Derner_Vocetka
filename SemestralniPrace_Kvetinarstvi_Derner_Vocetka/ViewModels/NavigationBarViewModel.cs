using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
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
        public ICommand NavigateMainCommand { get; }

        public ObservableCollection<string> ComboBoxItems { get; set; }

        private string selectedComboBoxItem;
        private ComboBoxTableNamesEnum selectedEnumValue;

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

        //todo finish setting up an account
        private readonly AccountStore accountStore;

        public bool IsLoggedIn => accountStore.IsLoggedIn;
        public bool IsLoggedOut => accountStore.IsLoggedOut;
        

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
            this.accountStore = accountStore;
            ComboBoxItems = new ObservableCollection<string>();
            PopulateComboBox();

            this.accountStore.CurrentAccountChanged += OnCurrentAccountChanged;
        }

        private void Logout()
        {
            accountStore.Logout();
            NavigateMainCommand.Execute(null);
            //todo navigate home command invoke
        }

        private void OnCurrentAccountChanged()
        {
            OnPropertyChanged(nameof(IsLoggedIn));
            OnPropertyChanged(nameof(IsLoggedOut));
        }

        private void PopulateComboBox()
        {
            //populating ComboBox with the ComboBoxTableNamesEnums descriptions 
            foreach (ComboBoxTableNamesEnum value in Enum.GetValues(typeof(ComboBoxTableNamesEnum)))
            {
                FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
                DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

                string description = (attributes.Length > 0) ? attributes[0].Description : value.ToString();
                ComboBoxItems.Add(description);
            }
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
            }
        }

        public override void Dispose()
        {
            this.accountStore.CurrentAccountChanged -= OnCurrentAccountChanged;
            base.Dispose();
        }

    }
}
