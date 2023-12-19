using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class InvoiceRepository : IRepository<Invoice>
    {
        public ObservableCollection<Invoice> Invoices { get; set; }
        private OracleDbUtil dbUtil;
        
        public InvoiceRepository()
        {
            Invoices = new ObservableCollection<Invoice>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Invoice> GetById(Int32 id)
        {
            string command = "GET_FAKTURY_BY_ID";
            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            byte[] pdfBytes = row["FAKTURA_PDF"] as byte[] ?? new byte[0]; // Get the image bytes or empty byte array if null
            var invoice = new Invoice(
                Convert.ToInt32(row["ID_FAKTURY"]),
                Convert.ToDateTime(row["DATUM"]),
                Convert.ToInt32(row["CENA"]),
                row["OBJEDNAVKY_ID_OBJEDNAVKA"] != DBNull.Value ? Convert.ToInt32(row["OrderId"]) : (int?)null,
                pdfBytes
            );
            
            return (Invoice)Convert.ChangeType(invoice, typeof(Invoice));
        }

        public async Task GetAll()
        {
            Invoices.Clear();
            string command = "GET_ALL_FAKTURY";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                byte[] pdfBytes = row["FAKTURA_PDF"] as byte[] ?? new byte[0];
                var invoice = new Invoice(
                    Convert.ToInt32(row["ID_FAKTURY"]),
                    Convert.ToDateTime(row["DATUM"]),
                    Convert.ToInt32(row["CENA"]),
                    Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKY"]),
                    pdfBytes
                );
                Invoices.Add(invoice);
            }
        }

        public async Task Add(Invoice entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "date", entity.Date },
                { "price", entity.Price }
            };
            await dbUtil.ExecuteStoredProcedureAsync("AddData.addfaktura", parameters);
        }

        public async Task Update(Invoice entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id", entity.Id },
                { "date", entity.Date },
                { "price", entity.Price }
            };
            await dbUtil.ExecuteStoredProcedureAsync("UpdateData.updatefaktura", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id", id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("DeleteData.deletefaktura", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Datum", typeof(DateTime));
            dataTable.Columns.Add("Cena", typeof(int));
            dataTable.Columns.Add("ID_ORDER", typeof(int));
            dataTable.Columns.Add("PDF", typeof(byte[]));

            foreach (var invoice in Invoices)
            {
                dataTable.Rows.Add(
                    invoice.Id,
                    invoice.Date, 
                    invoice.Price,
                    invoice.OrderId,
                    invoice.InvoicePdf);
            }
            
            return dataTable;
        }

        public List<Invoice> GetInvoices()
        {
            Task.Run(async () => await GetAll()).Wait();
            var invoices = Invoices.ToList();

            return invoices;
        }
    }
}