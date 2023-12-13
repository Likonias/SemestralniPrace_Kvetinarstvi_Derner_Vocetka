﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressViewModel : ViewModelBase
    {

        private OracleDbUtil dbUtil;
        private DataTable tableData;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        private bool isAnonymous;
        public bool IsAnonymous { get { return isAnonymous; } set { isAnonymous = value; OnPropertyChanged(nameof(IsAnonymous)); } }
        private DataRowView selectedItem;
        public DataRowView SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (SelectedItem.Row[1].ToString() == "anonymous")
                {
                    IsAnonymous = false;
                }
                else
                {
                    IsAnonymous = true;
                }
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private INavigationService createAddressForm;
        private AddressStore addressStore;
        private AddressRepository addressRepository;
        private AccountStore accountStore;
        public AddressViewModel(INavigationService createAddressForm, AddressStore addressStore, AccountStore accountStore)
        {
            this.createAddressForm = createAddressForm;
            this.accountStore = accountStore;
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            this.addressStore = addressStore;
            tableData = new DataTable();
            addressRepository = new AddressRepository(accountStore.CurrentAccount.EmployeePosition == Models.Enums.EmployeePositionEnum.ADMIN);
            InitializeTableData();
            IsAnonymous = true;
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await addressRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                addressStore.Address = await addressRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createAddressForm.Navigate();
            }
        }
        private void BtnAddPresseed()
        {

            addressStore.Address = null;
            createAddressForm.Navigate();
            
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await addressRepository.ConvertToDataTable();
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                FilterTableData();
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private void FilterTableData()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                InitializeTableData(); // Reset to the original data if search query is empty
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"Street LIKE '%{SearchQuery}%' OR " +
                   $"StreetNumber LIKE '%{SearchQuery}%' OR " +
                   $"City LIKE '%{SearchQuery}%' OR " +
                   $"Zip LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
