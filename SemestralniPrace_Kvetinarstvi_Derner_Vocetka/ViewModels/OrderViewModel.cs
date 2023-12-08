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
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private OrderStore orderStore;
        private DataTable tableData;
        private OracleDbUtil dbUtil;
        public RelayCommand BtnFlowers { get; }
        public RelayCommand BtnOthers { get; }
        public RelayCommand BtnCreateOrder { get; }
        private INavigationService createOrderFlower;
        private INavigationService createOrderOther;
        private INavigationService createOrderFormView;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public DataRowView SelectedItem { get; set; }
        private AccountStore accountStore;
        public OrderViewModel(INavigationService navigationService, OrderStore orderStore, INavigationService createOrderFlower, INavigationService createOrderOther, INavigationService createOrderFormView, AccountStore accountStore)
        {
            this.navigationService = navigationService;
            this.orderStore = orderStore;
            this.createOrderFlower = createOrderFlower;
            this.createOrderOther = createOrderOther;
            this.createOrderFormView = createOrderFormView;
            this.accountStore = accountStore;
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            BtnFlowers = new RelayCommand(BtnFlowersClicked);
            BtnOthers = new RelayCommand(BtnOthersClicked);
            BtnCreateOrder = new RelayCommand(BtnCreateOrderClicked);
            InitializeTableData();
        }

        private void BtnCreateOrderClicked()
        {
            orderStore.IdAccount = accountStore.CurrentAccount.Id;
            orderStore.Email = accountStore.CurrentAccount.Email;
            if(accountStore.CurrentAccount.EmployeePosition == null )
            {
                orderStore.IsCustomer = true;
            }
            else
            {
                orderStore.IsCustomer = false;
            }
            createOrderFormView.Navigate();
        }

        private void BtnOthersClicked()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                orderStore.Id = int.Parse(SelectedItem.Row[0].ToString());
                createOrderOther.Navigate();
            }
        }

        private void BtnFlowersClicked()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                orderStore.Id = int.Parse(SelectedItem.Row[0].ToString());
                createOrderFlower.Navigate();
            }
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await dbUtil.LoadDataFromViewAsync("OBJEDNAVKY_VIEW");
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
                // Reset to the original data if search query is empty
                InitializeTableData();
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"CONVERT(ID_OBJEDNAVKY, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(CELKOVA_CENA, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ID_STAV, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(DATUM_UHRADY, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(DATUM_PRIJETI, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ID_PLATBA, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(DRUH_PLATBY, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(POZNAMKA, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ID_ZAKAZNIK, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ZAKAZNIK_FULL_NAME, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ZAKAZNIK_EMAIL, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ZAMESTNANEC_FULL_NAME, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(ZAMESTNANEC_EMAIL, 'System.String') LIKE '%{SearchQuery}%' OR " +
                           $"CONVERT(DRUH_PRILEZITOSTI, 'System.String') LIKE '%{SearchQuery}%'";

            TableData = dv.ToTable();
        }


    }
}
