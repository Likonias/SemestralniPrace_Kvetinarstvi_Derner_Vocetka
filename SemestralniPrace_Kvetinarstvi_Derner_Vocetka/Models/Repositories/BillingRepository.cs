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
    public class BillingRepository : IRepository<Billing>
    {
        public ObservableCollection<Billing> Billings { get; set; }
        private OracleDbUtil dbUtil;

        public BillingRepository()
        {
            Billings = new ObservableCollection<Billing>();
            dbUtil = new OracleDbUtil();
        }

        public async Task Add(Billing entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "DRUH_PLATBY", entity.BillingType.ToString() },
                { "POZNAMKA", entity.Note}
            };
            await dbUtil.ExecuteStoredProcedureAsync("addplatby", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_PLATBA", id}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteplatby", parameters);
        }

        public async Task GetAll()
        {
            Billings.Clear();
            string command = "SELECT * FROM platby";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                if (Enum.TryParse(row["DRUH_PLATBY"].ToString(), out BillingTypeEnum billingType))
                {
                    var billing = new Billing(
                        Convert.ToInt32(row["ID_PLATBA"]),
                        billingType,
                        row["POZNAMKA"].ToString()
                    );
                    Billings.Add(billing);
                }
            }
        }

        public async Task<Billing> GetById(int id)
        {
            string command = $"SELECT * FROM platby WHERE ID_PLATBA = {id}";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var billing = new Billing(
                Convert.ToInt32(row["ID_PLATBA"]),
                (BillingTypeEnum)Enum.Parse(typeof(BillingTypeEnum), row["DRUH_PLATBY"].ToString()),
                row["POZNAMKA"].ToString()
                );

            return billing;
        }

        public async Task Update(Billing entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_PLATBA", entity.Id },
                { "DRUH_PLATBY", entity.BillingType.ToString() },
                { "POZNAMKA", entity.Note ?? null }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateplatby", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("IdBilling");
            dataTable.Columns.Add("BillingType");
            dataTable.Columns.Add("Note");

            foreach (var billing in Billings)
            {
                dataTable.Rows.Add(
                    billing.Id,
                    billing.BillingType,
                    billing.Note
                );
            }

            return dataTable;
        }

        public List<Billing> GetBillings()
        {
            Task.Run(async () => await GetAll()).Wait();
            var billings = Billings.ToList();

            return billings;
        }
    }
}
