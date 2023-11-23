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
        public ICommand NavigateViewCommand { get; }

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
            }
        }

        //todo finish setting up an account
        private readonly AccountStore accountStore;

        public NavigationBarViewModel(AccountStore accountStore, INavigationService<LoginViewModel> loginNavigationService, INavigationService<RegisterViewModel> registerNavigationService)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(registerNavigationService);
            this.accountStore = accountStore;
            ComboBoxItems = new ObservableCollection<string>();
            PopulateComboBox();

            //TODO implement account
            //NavigateAccountCommand =
            //NavigateViewCommand = 
            
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
                    
                    break;
            }
        }

        //private void LoadSelectedTableData()
        //{
        //    if (!string.IsNullOrEmpty(SelectedTableName))
        //    {
        //        // Fetch data for the selected table using OracleDbUtil
        //        // Example:
        //        // SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM {SelectedTableName}");
        //        SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM zakaznici");

        //    }
        //    SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM zakaznici");
        //}
    }
}
