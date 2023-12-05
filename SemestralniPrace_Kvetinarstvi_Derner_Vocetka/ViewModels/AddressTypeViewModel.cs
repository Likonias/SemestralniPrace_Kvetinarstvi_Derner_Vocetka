﻿using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressTypeViewModel : ViewModelBase
    {
        private readonly AddressTypeStore addressTypeStore;
        private readonly AddressTypeRepository addressTypeRepository;
        private DataTable tableData;

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
        public ObservableCollection<AddressType> AddressTypes { get; set; }

        public AddressTypeViewModel(AddressTypeStore addressTypeStore)
        {
            this.addressTypeStore = addressTypeStore;
            addressTypeRepository = new AddressTypeRepository();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await addressTypeRepository.ConvertToDataTable();

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
