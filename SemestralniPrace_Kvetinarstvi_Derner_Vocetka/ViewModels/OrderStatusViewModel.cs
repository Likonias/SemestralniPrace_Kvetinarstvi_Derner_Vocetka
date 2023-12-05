using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderStatusViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;
        private OrderStatusRepository orderStatusRepository;
        private OrderStatusStore orderStatusStore;
        private INavigationService createOrderStatusForm;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        public DataRowView SelectedItem { get; set; }

        private DataTable tableData;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public OrderStatusViewModel(INavigationService createOrderStatusForm, OrderStatusStore orderStatusStore)
        {
            this.createOrderStatusForm = createOrderStatusForm;
            this.orderStatusStore = orderStatusStore;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            orderStatusRepository = new OrderStatusRepository();
            tableData = new DataTable();
            InitializeOrderStatuses();
        }

        private async void InitializeOrderStatuses()
        {
            TableData = new DataTable();
            TableData = await orderStatusRepository.ConvertToDataTable();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem != null)
            {
                int orderStatusIdToDelete = int.Parse(SelectedItem.Row[0].ToString());
                await orderStatusRepository.Delete(orderStatusIdToDelete);
                InitializeOrderStatuses();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem != null)
            {
                orderStatusStore.OrderStatus = await orderStatusRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createOrderStatusForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            orderStatusStore.OrderStatus = null;
            createOrderStatusForm.Navigate();
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
                InitializeOrderStatuses(); // Reset to the original data if search query is empty
                return;
            }

            DataView dv = TableData.DefaultView;
            dv.RowFilter = $"CONVERT(Id, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(OrderDate, 'System.String') LIKE '%{SearchQuery}%' OR " +
                   $"CONVERT(PaymentDate, 'System.String') LIKE '%{SearchQuery}%'";

            TableData = dv.ToTable();
        }
    }
}
