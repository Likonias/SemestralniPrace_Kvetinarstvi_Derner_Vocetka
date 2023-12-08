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
    public class DeliveryRepository
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
            string command = $"SELECT * FROM doruceni WHERE " +
                             $"JOIN ZPUSOBY_PREVZETI on ZPUSOBY_PREVZETI.id_zpusob_prevzeti = doruceni.id_zpusob_prevzeti" +
                             $" ID_DORUCENI = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var delivery = new Delivery(
                Convert.ToInt32(row["id_zpusob_prevzeti"]),
                Convert.ToDateTime(row["DATUM_VYDANI"]),
                Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKY"]),
                (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString()),
                Convert.ToInt32(row["ID_DORUCENI"]),
                row["SPOLECNOST"].ToString()
            );

            return delivery;
        }


        public async Task GetAll()
        {
            Deliveries.Clear();
            string command = $"SELECT * FROM doruceni " +
                             $"JOIN ZPUSOBY_PREVZETI on ZPUSOBY_PREVZETI.id_zpusob_prevzeti = doruceni.id_zpusob_prevzeti";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var delivery = new Delivery(
                    Convert.ToInt32(row["id_zpusob_prevzeti"]),
                    Convert.ToDateTime(row["DATUM_VYDANI"]),
                    Convert.ToInt32(row["OBJEDNAVKY_ID_OBJEDNAVKY"]),
                    (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString()),
                    Convert.ToInt32(row["ID_DORUCENI"]),
                    row["SPOLECNOST"].ToString()
                );

                Deliveries.Add(delivery);
            }
        }

        public async Task Add(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"DATUM_VYDANI", entity.WarehouseReleaseDate },
                {"OBJEDNAVKY_ID_OBJEDNAVKA" , entity.IdOrder},
                {"TYP", entity.Method },
                { "SPOLECNOST", entity.TransportCompany },
            };
            await dbUtil.ExecuteStoredProcedureAsync("adddoruceni", parameters);
        }

        public async Task Update(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id_zpusob_prevzeti", entity.IdDeliveryMethod },
                {"DATUM_VYDANI", entity.WarehouseReleaseDate },
                {"OBJEDNAVKY_ID_OBJEDNAVKA" , entity.IdOrder},
                {"TYP", entity.Method },
                { "ID_DORUCENI", entity.IdDelivery },
                { "SPOLECNOST", entity.TransportCompany },
            };
        }

        public async Task Delete(Delivery entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "id_zpusob_prevzeti", entity.IdDeliveryMethod },
                { "ID_DORUCENI", entity.IdDelivery }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletedoruceni", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("ID_DORUCENI", typeof(int));
            dataTable.Columns.Add("DATUM_VYDANI", typeof(DateTime));
            dataTable.Columns.Add("OBJEDNAVKY_ID_OBJEDNAVKA", typeof(int));
            dataTable.Columns.Add("TYP", typeof(DeliveryMethodEnum)); // Assuming DeliveryMethodEnum is the correct type
            dataTable.Columns.Add("SPOLECNOST", typeof(string));

            foreach (var delivery in Deliveries)
            {
                DataRow row = dataTable.NewRow();
                row["ID_DORUCENI"] = delivery.IdDelivery;
                row["DATUM_VYDANI"] = delivery.WarehouseReleaseDate;
                row["OBJEDNAVKY_ID_OBJEDNAVKA"] = delivery.IdOrder;
                row["TYP"] = delivery.Method;
                row["SPOLECNOST"] = delivery.TransportCompany;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public List<Delivery> GetDeliveries() 
        {
            Task.Run(async () => await GetAll()).Wait();
            var deliveries = Deliveries.ToList();

            return deliveries;  
        }
    }
}