using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
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

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class FlowersViewModel : ViewModelBase
    {

        private OracleDbUtil dbUtil;
        private DataTable tableData;
        private FlowerRepository flowerRepository;
        private FlowerStore flowerStore;
        private INavigationService createFlowersForm;

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
        private bool inNotCustomer;
        public bool IsNotCustomer
        {
            get { return inNotCustomer; }
            set
            {
                inNotCustomer = value;
                OnPropertyChanged("IsNotCustomer");
            }
        }
        private AccountStore AccountStore;
        public FlowersViewModel(INavigationService createFlowersForm, FlowerStore flowerStore, AccountStore accountStore)
        {
            this.createFlowersForm = createFlowersForm;
            this.flowerStore = flowerStore;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            flowerRepository = new FlowerRepository();
            InitializeTableData();
            AccountStore = accountStore;
            if (accountStore.CurrentAccount.EmployeePosition == null) { IsNotCustomer = false; } else { IsNotCustomer = true; }
        }
        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await flowerRepository.ConvertToDataTable();
        }
        public override void Dispose()
        {
            base.Dispose();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                int flowerIdToDelete = int.Parse(SelectedItem.Row[0].ToString());
                Flower flowerToDelete = await flowerRepository.GetById(flowerIdToDelete);
                await flowerRepository.Delete(flowerToDelete);
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                flowerStore.Flower = await flowerRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createFlowersForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            flowerStore.Flower = null;
            createFlowersForm.Navigate();
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
            dv.RowFilter = $"CONVERT(Name, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Price, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Warehouse, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(FlowerState, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Age, 'System.String') LIKE '%{SearchQuery}%' OR " +                 
                   $"CONVERT(Image_name, 'System.String') LIKE '%{SearchQuery}%'";

            TableData = dv.ToTable();
        }
    }
}
