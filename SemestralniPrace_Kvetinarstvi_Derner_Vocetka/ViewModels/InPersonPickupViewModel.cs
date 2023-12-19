using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InPersonPickupViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;
        private DataTable tableData;
        private InPersonPickupRepository inPersonPickupRepository;
        private InPersonPickupStore inPersonPickupStore;
        private INavigationService createInPersonPickupForm;

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

        public InPersonPickupViewModel(INavigationService createInPersonPickupForm, InPersonPickupStore inPersonPickupStore)
        {
            this.createInPersonPickupForm = createInPersonPickupForm;
            this.inPersonPickupStore = inPersonPickupStore;
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            inPersonPickupRepository = new InPersonPickupRepository();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await inPersonPickupRepository.ConvertToDataTable();
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
                InitializeTableData(); // Reset to the original data if the search query is empty
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"CONVERT(IdDeliveryMethod, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(WarehouseReleaseDate, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(IdOrder, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Method, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(IdPickup, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(Time, 'System.String') LIKE '%{SearchQuery}%'";

            TableData = dv.ToTable();
        }
    }
}
