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
    public class DeliveryMethodRepository : IRepository<DeliveryMethod>
    {
        public ObservableCollection<DeliveryMethod> DeliveryMethods { get; set; }
        private OracleDbUtil dbUtil;
        
        public DeliveryMethodRepository()
        {
            DeliveryMethods = new ObservableCollection<DeliveryMethod>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<DeliveryMethod> GetById(Int32 id)
        {
            string command = $"SELECT * FROM doruceni WHERE ID_DORUCENI = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var deliveryMethod = new DeliveryMethod(
                Convert.ToInt32(row["ID_DORUCENI"]),
                Convert.ToDateTime(row["DATUM_VYDANI"]),
                row["OBJEDNAVKY_ID_OBJEDNAVKA"] != DBNull.Value ? Convert.ToInt32(row["OrderId"]) : (int?)null,
                (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString())
            );
            
            return (DeliveryMethod)Convert.ChangeType(deliveryMethod, typeof(DeliveryMethod));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM doruceni";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var deliveryMethod = new DeliveryMethod(
                    Convert.ToInt32(row["ID_DORUCENI"]),
                    Convert.ToDateTime(row["DATUM_VYDANI"]),
                    row["OBJEDNAVKY_ID_OBJEDNAVKA"] != DBNull.Value ? Convert.ToInt32(row["OrderId"]) : (int?)null,
                    (DeliveryMethodEnum)Enum.Parse(typeof(DeliveryMethodEnum), row["TYP"].ToString())
                );
                DeliveryMethods.Add(deliveryMethod);
            }
        }

        public async Task Add(DeliveryMethod entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "DATUM_VYDANI", entity.WarehouseReleaseDate },
                { "TYP", entity.Method.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("adddoruceni", parameters);
        }

        public async Task Update(DeliveryMethod entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DORUCENI", entity.IdDeliveryMethod },
                { "DATUM_VYDANI", entity.WarehouseReleaseDate },
                { "TYP", entity.Method.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatedoruceni", parameters);
        }

        public async Task Delete(DeliveryMethod entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DORUCENI", entity.IdDeliveryMethod }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletedoruceni", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_DORUCENI", typeof(int));
            dataTable.Columns.Add("DATUM_VYDANI", typeof(DateTime));
            dataTable.Columns.Add("TYP", typeof(DeliveryMethodEnum));
            
            foreach (var deliveryMethod in DeliveryMethods)
            {
                DataRow row = dataTable.NewRow();
                row["ID_DORUCENI"] = deliveryMethod.IdDeliveryMethod;
                row["DATUM_VYDANI"] = deliveryMethod.WarehouseReleaseDate;
                row["TYP"] = deliveryMethod.Method;
                dataTable.Rows.Add(row);
            }
            
            return dataTable;
        }

        public List<DeliveryMethod> GetDeliveryMethods()
        {
            Task.Run(async () => await GetAll()).Wait();
            var deliveryMethods = DeliveryMethods.ToList();

            return deliveryMethods;
        }
    }
}