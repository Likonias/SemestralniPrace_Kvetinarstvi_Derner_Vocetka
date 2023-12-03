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
    public class OrderRepository : IRepository<Order>
    {
        public ObservableCollection<Order> Orders { get; set; }
        private OracleDbUtil dbUtil;
        
        public OrderRepository()
        {
            Billings = new ObservableCollection<Order>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Order> GetById(Int32 id)
        {
            string command = $"SELECT * FROM objednavky WHERE ID_OBJEDNAVKA = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var order = new Order(
                Convert.ToInt32(row["ID_OBJEDNAVKA"]),
                Convert.ToInt32(row["CELKOVA_CENA"]),
                null,
                null,
                null, //new CustomerRepository().GetById(Convert.ToInt32(row["ZAKAZNICI_ID_ZAKAZNIK"])),
                null
            );
            
            return (Order)Convert.ChangeType(order, typeof(Order));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM objednavky";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var order = new Order(
                    Convert.ToInt32(row["ID_OBJEDNAVKA"]),
                    Convert.ToInt32(row["CELKOVA_CENA"]),
                    null,
                    null,
                    null,
                    null
                );
                Orders.Add(order);
            }
        }

        public async Task Add(Order entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"CELKOVA_CENA", entity.FinalPrice}
            };
            await dbUtil.ExecuteStoredProcedureAsync("addobjednavky", parameters);
        }

        public async Task Update(Order entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_OBJEDNAVKA", entity.Id},
                {"CELKOVA_CENA", entity.FinalPrice}
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateobjednavky", parameters);
        }

        public async Task Delete(Order entity)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ID_OBJEDNAVKA", entity.Id}
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteobjednavky", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_OBJEDNAVKA", typeof(int));
            dataTable.Columns.Add("CELKOVA_CENA", typeof(int));
            
            foreach (var order in Orders)
            {
                dataTable.Rows.Add(order.Id, order.FinalPrice);
            }
            
            return dataTable;
        }

        public List<Order> GetOrders()
        {
            Task.Run(async () => await GetAll()).Wait();
            var orders = Orders.ToList();

            return orders;
        }
    }
}