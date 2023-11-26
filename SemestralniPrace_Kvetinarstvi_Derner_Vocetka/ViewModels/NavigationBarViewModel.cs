using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
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

        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateRegisterCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateFlowersCommand { get; }
        
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

        public NavigationBarViewModel(AccountStore accountStore, NavigationServiceManager navigationServiceManager)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigationServiceManager.GetNavigationService<LoginViewModel>());
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(navigationServiceManager.GetNavigationService<RegisterViewModel>());
            NavigateAccountCommand = new NavigateCommand<AccountViewModel>(navigationServiceManager.GetNavigationService<AccountViewModel>());
            NavigateFlowersCommand = new NavigateCommand<FlowersViewModel>(navigationServiceManager.GetNavigationService<FlowersViewModel>());

            this.accountStore = accountStore;
            ComboBoxItems = new ObservableCollection<string>();
            PopulateComboBox();

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
            }
        }

    }
}
