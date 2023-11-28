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
            string command = $"SELECT * FROM faktury WHERE ID_FAKTURA = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var invoice = new Invoice(
                Convert.ToInt32(row["ID_FAKTURA"]),
                Convert.ToDateTime(row["DATUM"]),
                Convert.ToInt32(row["CENA"]),
                row["OBJEDNAVKY_ID_OBJEDNAVKA"] != DBNull.Value ? Convert.ToInt32(row["OrderId"]) : (int?)null,
                null
            );
            
            return (Invoice)Convert.ChangeType(invoice, typeof(Invoice));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM faktury";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var invoice = new Invoice(
                    Convert.ToInt32(row["ID_FAKTURA"]),
                    Convert.ToDateTime(row["DATUM"]),
                    Convert.ToInt32(row["CENA"]),
                    row["OBJEDNAVKY_ID_OBJEDNAVKA"] != DBNull.Value ? Convert.ToInt32(row["OrderId"]) : (int?)null,
                    null
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
            await dbUtil.ExecuteStoredProcedureAsync("addfaktura", parameters);
        }

        public async Task Update(Invoice entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id", entity.Id },
                { "date", entity.Date },
                { "price", entity.Price }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatefaktura", parameters);
        }

        public async Task Delete(Invoice entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletefaktura", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("Datum", typeof(DateTime));
            dataTable.Columns.Add("Cena", typeof(int));
            
            foreach (var invoice in Invoices)
            {
                dataTable.Rows.Add(invoice.Date, invoice.Price);
            }
            
            return dataTable;
        }
    }
}