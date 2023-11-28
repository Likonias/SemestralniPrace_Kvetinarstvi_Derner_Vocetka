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
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class DeliveryRepository : IRepository<Delivery>
    {
        public ObservableCollection<Delivery> Deliveries { get; set; }
        private OracleDbUtil dbUtil;

        public DeliveryRepository()
        {
            Deliveries = new ObservableCollection<Delivery>();
            dbUtil = new OracleDbUtil();
        }

        public async Task<Delivery> GetById(Int32 id)
        {
            string command = $"SELECT * FROM doruceni WHERE ID_DORUCENI = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var delivery = new Delivery(
                Convert.ToInt32(row["ID_DORUCENI"]),
                Convert.ToDateTime(row["WarehouseReleaseDate"]),
                row["IdOrder"] != DBNull.Value ? Convert.ToInt32(row["IdOrder"]) : (int?)null,
                (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["Method"].ToString()),
                Convert.ToInt32(row["ID_DORUCENI"]),
                row["SPOLECNOST"].ToString()
            );

            // Set additional properties
            delivery.WarehouseReleaseDate = Convert.ToDateTime(row["WarehouseReleaseDate"]);
            delivery.IdOrder = row["IdOrder"] != DBNull.Value ? Convert.ToInt32(row["IdOrder"]) : (int?)null;
            delivery.Method = (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["Method"].ToString());

            return delivery;
        }


        public async Task GetAll()
        {
            string command = "SELECT * FROM doruceni";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var delivery = new Delivery(
                    Convert.ToInt32(row["ID_DORUCENI"]),
                    Convert.ToDateTime(row["WarehouseReleaseDate"]),
                    row["IdOrder"] != DBNull.Value ? Convert.ToInt32(row["IdOrder"]) : (int?)null,
                    (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["Method"].ToString()),
                    Convert.ToInt32(row["ID_DORUCENI"]),
                    row["SPOLECNOST"].ToString()
                );

                delivery.WarehouseReleaseDate = Convert.ToDateTime(row["WarehouseReleaseDate"]);
                delivery.IdOrder = row["IdOrder"] != DBNull.Value ? Convert.ToInt32(row["IdOrder"]) : (int?)null;
                delivery.Method = (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["Method"].ToString());

                Deliveries.Add(delivery);
            }
        }

        public async Task Add(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "SPOLECNOST", entity.TransportCompany },
            };
            await dbUtil.ExecuteStoredProcedureAsync("adddoruceni", parameters);
        }

        public async Task Update(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DORUCENI", entity.IdDelivery },
                { "SPOLECNOST", entity.TransportCompany },
            };
        }

        public async Task Delete(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DORUCENI", entity.IdDelivery }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletedoruceni", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("TransportCompany", typeof(string));

            foreach (var delivery in Deliveries)
            {
                DataRow row = dataTable.NewRow();
                row["TransportCompany"] = delivery.TransportCompany;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}