﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
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
    public class OrderOtherViewModel : ViewModelBase
    {
        public RelayCommand BtnClose { get; }
        private DataTable tableData;
        private OracleDbUtil dbUtil;
        private OrderStore orderStore;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public OrderOtherViewModel(OrderStore orderStore, INavigationService navigationService)
        {
            this.orderStore = orderStore;
            NavigationService = navigationService;
            BtnClose = new RelayCommand(BtnCloseClicked);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await dbUtil.ExecuteGetGoodsFunctionAsync("GetZboziByObjednavkaId", orderStore.Id, "O");
        }

        private void BtnCloseClicked()
        {
            NavigationService.Navigate();
        }

        public INavigationService NavigationService { get; }
    }
}
