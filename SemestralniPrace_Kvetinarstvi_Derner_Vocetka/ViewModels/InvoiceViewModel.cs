using Microsoft.Win32;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    //todo doesnt dowload pdf
    public class InvoiceViewModel : ViewModelBase
    {
        private InvoiceStore invoiceStore;
        private Models.Invoice invoice;
        public DataRowView SelectedItem { get; set; }
        public RelayCommand BtnDownloadPdf { get; }
        private DataTable tableData;
        private OracleDbUtil dbUtil;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private InvoiceRepository invoiceRepository;
        private INavigationService createInvoiceForm;
        private AccountStore accountStore;
        public InvoiceViewModel(INavigationService createInvoiceForm, InvoiceStore invoiceStore, AccountStore accountStore)
        {
            dbUtil = new OracleDbUtil();
            this.createInvoiceForm = createInvoiceForm;
            BtnDownloadPdf = new RelayCommand(BtnDownloadPdfPressed);
            invoiceRepository = new InvoiceRepository();
            this.invoiceStore = invoiceStore;
            tableData = new DataTable();
            this.accountStore = accountStore;

            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            if (accountStore.CurrentAccount.EmployeePosition == null)
            {
                DataTable dt = await dbUtil.GetInvoicesForCustomer(accountStore.CurrentAccount.Id);

                TableData.Columns.Add("Id", typeof(int));
                TableData.Columns.Add("Datum", typeof(DateTime));
                TableData.Columns.Add("Cena", typeof(int));
                TableData.Columns.Add("ID_ORDER", typeof(int));
                TableData.Columns.Add("PDF", typeof(byte[]));

                foreach (DataRow row in dt.Rows)
                {
                    byte[] pdfBytes = row["FAKTURA_PDF"] as byte[] ?? new byte[0];

                    TableData.Rows.Add(
                            Convert.ToInt32(row["ID_FAKTURY"]),
                            Convert.ToDateTime(row["DATUM"]),
                            Convert.ToInt32(row["CENA"]),
                            Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKY"]),
                            pdfBytes
                        );
                }
            }
            else
            {
                TableData = await invoiceRepository.ConvertToDataTable();
            }
        }

        private async void BtnDownloadPdfPressed()
        {
            int invoiceId = Convert.ToInt32(SelectedItem.Row["Id"]);
            string fileName = await GetFileNameFromBlobInfo(invoiceId, "FAKTURY");

            if (SelectedItem != null)
            {
                // Assuming that the "PDF" column in your DataTable contains byte[] data
                byte[] pdfBytes = (byte[])SelectedItem.Row["PDF"];

                if (pdfBytes != null && pdfBytes.Length > 0)
                {
                    // Prompt the user to choose a file location
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Word files (*.docx)|*.docx";
                    saveFileDialog.FileName = $"{fileName}.docx";  // Default file name based on invoice ID

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        // Save the PDF bytes to the chosen location
                        File.WriteAllBytes(saveFileDialog.FileName, pdfBytes);

                        // Optionally, you can display a message to the user indicating that the download was successful.
                        MessageBox.Show("PDF saved successfully!", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    // Optionally, handle the case where the PDF data is empty or null.
                    MessageBox.Show("No PDF data available for saving.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
                            $"CONVERT(Price, 'System.String') LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }

        public async Task<string> GetFileNameFromBlobInfo(int foreignId, string tableName)
        {
            string sql = $"SELECT NAME FROM BLOB_INFO WHERE FOREIGN_ID = {foreignId} AND TABLE_NAME = '{tableName}'";

            var dataTable = await dbUtil.ExecuteQueryAsync(sql);

            if (dataTable.Rows.Count == 0)
            {
                return "faktura";

            }
            else
            {
                var row = dataTable.Rows[0];
                return row["NAME"].ToString();
            }


        }
    }
}
