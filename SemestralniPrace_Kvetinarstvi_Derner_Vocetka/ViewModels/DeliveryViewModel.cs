using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Threading.Tasks;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System.Data;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryViewModel : ViewModelBase
    {
        private readonly DeliveryStore deliveryStore;
        private readonly DeliveryRepository deliveryRepository;
        private INavigationService createDeliveryForm;
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

        public DeliveryViewModel(INavigationService createDeliveryForm, DeliveryStore deliveryStore)
        {
            this.createDeliveryForm = createDeliveryForm;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            this.deliveryStore = deliveryStore;
            deliveryRepository = new DeliveryRepository();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                int deliveryIdToDelete = int.Parse(SelectedItem.Row[0].ToString());
                Delivery deliveryToDelete = await deliveryRepository.GetById(deliveryIdToDelete);
                deliveryRepository.Delete(deliveryToDelete);
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                deliveryStore.Delivery = await deliveryRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createDeliveryForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            deliveryStore.Delivery = null;
            createDeliveryForm.Navigate();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await deliveryRepository.ConvertToDataTable();
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
            dv.RowFilter = $"TransportCompany LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
