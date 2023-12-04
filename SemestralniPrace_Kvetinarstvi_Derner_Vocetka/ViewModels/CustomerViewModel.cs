﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {

        private CustomerStore customerStore;
        private OracleDbUtil dbUtil;
        private DataTable tableData;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        public DataRowView SelectedItem { get; set; }
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private INavigationService createCustomerForm;
        private CustomerRepository customerRepository;
        public CustomerViewModel(INavigationService createCustomerForm, CustomerStore customerStore)
        {
            this.customerStore = customerStore;
            this.createCustomerForm = createCustomerForm;
            customerRepository = new CustomerRepository();
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await customerRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                customerStore.Customer = await customerRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createCustomerForm.Navigate();
            }



        }
        private void BtnAddPresseed()
        {

            customerStore.Customer = null;
            createCustomerForm.Navigate();

        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await customerRepository.ConvertToDataTable();

        }

        public override void Dispose()
        {
            base.Dispose();
        }


    }
}
