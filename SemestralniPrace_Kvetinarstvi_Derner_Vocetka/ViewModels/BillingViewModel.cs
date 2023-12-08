using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class BillingViewModel : ViewModelBase
    {
        //TODO NOT FINISHED
        private readonly BillingStore billingStore;
        private readonly BillingRepository billingRepository;
        private INavigationService createBillingForm;
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
        public ObservableCollection<Billing> Billings { get; set; }

        public BillingViewModel(INavigationService createBillingForm, BillingStore billingStore)
        {
            this.createBillingForm = createBillingForm;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            this.billingStore = billingStore;
            billingRepository = new BillingRepository();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                billingRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                billingStore.Billing = await billingRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createBillingForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            billingStore.Billing = null;
            createBillingForm.Navigate();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await billingRepository.ConvertToDataTable();
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
                InitializeTableData(); // Reset to the original data if the search query is empty
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"BillingType LIKE '%{SearchQuery}%' OR " +
                   $"Note LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
