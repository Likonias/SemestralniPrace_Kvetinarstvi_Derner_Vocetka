using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OccasionViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;
        private DataTable tableData;
        private OccasionRepository occasionRepository;
        private OccasionStore occasionStore;
        private INavigationService createOccasionForm;

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

        public OccasionViewModel(INavigationService createOccasionForm, OccasionStore occasionStore)
        {
            this.createOccasionForm = createOccasionForm;
            this.occasionStore = occasionStore;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            occasionRepository = new OccasionRepository();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await occasionRepository.ConvertToDataTable();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                int occasionIdToDelete = int.Parse(SelectedItem.Row[0].ToString());
                await occasionRepository.Delete(occasionIdToDelete);
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                occasionStore.Occasion = await occasionRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createOccasionForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            occasionStore.Occasion = null;
            createOccasionForm.Navigate();
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
            dv.RowFilter = $"CONVERT(Id, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(OccasionType, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Note, 'System.String') LIKE '%{SearchQuery}%'";

            TableData = dv.ToTable();
        }
    }
}
