using Microsoft.Win32;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        private readonly Navigation.Stores.InvoiceStore invoiceStore;
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
        public InvoiceViewModel(INavigationService createInvoiceForm, InvoiceStore invoiceStore)
        {
            dbUtil = new OracleDbUtil();
            this.createInvoiceForm = createInvoiceForm;
            BtnDownloadPdf = new RelayCommand(BtnDownloadPdfPressed);
            invoiceRepository = new InvoiceRepository();
            this.invoiceStore = invoiceStore;
            tableData = new DataTable();

            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await invoiceRepository.ConvertToDataTable();
        }

        private async void BtnDownloadPdfPressed()
        {
            int invoiceId = Convert.ToInt32(SelectedItem.Row["Id"]);
            string fileName = await GetFileNameFromBlobInfo(invoiceId, "faktury");

            if (SelectedItem != null)
            {
                // Assuming that the "PDF" column in your DataTable contains byte[] data
                byte[] pdfBytes = (byte[])SelectedItem.Row["PDF"];

                if (pdfBytes != null && pdfBytes.Length > 0)
                {
                    // Convert the PDF byte array to a Base64 string
                    string base64Pdf = Convert.ToBase64String(pdfBytes);

                    // Prompt the user to choose a file location
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Text files (*.txt)|*.txt";
                    saveFileDialog.FileName = $"{fileName}.txt"; // Default file name based on invoice ID

                    if (saveFileDialog.ShowDialog() == true)
                    {
                        // Save the Base64 string to the chosen location
                        File.WriteAllText(saveFileDialog.FileName, base64Pdf);

                        // Optionally, you can display a message to the user indicating that the download was successful.
                        MessageBox.Show("PDF saved as text successfully!", "Save Complete", MessageBoxButton.OK, MessageBoxImage.Information);
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
