using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryMethodViewModel : ViewModelBase
    {
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

        private INavigationService createDeliveryMethodForm;
        private DeliveryMethodStore deliveryMethodStore;
        private DeliveryMethodRepository deliveryMethodRepository;

        public DeliveryMethodViewModel(INavigationService createDeliveryMethodForm, DeliveryMethodStore deliveryMethodStore)
        {
            this.createDeliveryMethodForm = createDeliveryMethodForm;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            this.deliveryMethodStore = deliveryMethodStore;
            tableData = new DataTable();
            deliveryMethodRepository = new DeliveryMethodRepository();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await deliveryMethodRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                deliveryMethodStore.DeliveryMethod = await deliveryMethodRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createDeliveryMethodForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            deliveryMethodStore.DeliveryMethod = null;
            createDeliveryMethodForm.Navigate();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await deliveryMethodRepository.ConvertToDataTable();
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
            dv.RowFilter = $"WarehouseReleaseDate LIKE '%{SearchQuery}%' OR " +
                   $"IdOrder LIKE '%{SearchQuery}%' OR " +
                   $"Method LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
