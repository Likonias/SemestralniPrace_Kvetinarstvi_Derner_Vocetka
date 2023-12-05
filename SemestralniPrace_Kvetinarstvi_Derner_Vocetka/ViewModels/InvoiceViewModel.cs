using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        private readonly Navigation.Stores.InvoiceStore invoiceStore;
        private Models.Invoice invoice;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        public DataRowView SelectedItem { get; set; }
        public DataTable TableData { get; set; }
        private InvoiceRepository invoiceRepository;
        private INavigationService createInvoiceForm;
        public InvoiceViewModel(INavigationService createInvoiceForm, InvoiceStore invoiceStore)
        {
            this.createInvoiceForm = createInvoiceForm;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            invoiceRepository = new InvoiceRepository();
            this.invoiceStore = invoiceStore;
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            // Call your method to populate the TableData with invoice data
            // E.g., TableData = await invoiceRepository.ConvertToDataTable();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await invoiceRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                invoiceStore.Invoice = await invoiceRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createInvoiceForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            invoiceStore.Invoice = null;
            createInvoiceForm.Navigate();
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

            DataView dv = TableData.DefaultView;
            dv.RowFilter = $"CONVERT(Date, 'System.String') LIKE '%{SearchQuery}%' OR " +
                            $"CONVERT(Price, 'System.String') LIKE '%{SearchQuery}%' OR " +
                            $"CONVERT(OrderId, 'System.String') LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
