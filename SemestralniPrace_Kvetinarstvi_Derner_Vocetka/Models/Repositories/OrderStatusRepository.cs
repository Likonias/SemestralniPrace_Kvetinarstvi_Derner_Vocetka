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
    public class OrderStatusRepository : IRepository<OrderStatus>
    {
        public ObservableCollection<OrderStatus> OrderStatuses { get; set; }
        private OracleDbUtil dbUtil;
        
        public OrderStatusRepository()
        {
            OrderStatuses = new ObservableCollection<OrderStatus>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<T> GetById(Int32 id)
        {
            string command = $"SELECT * FROM stavy WHERE ID_STAV = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;
            
            var row = dataTable.Rows[0];
            var orderStatus = new OrderStatus(
                Convert.ToInt32(row["ID_STAV"]),
                Convert.ToDateTime(row["DATUM_PRIJETI"]),
                Convert.ToDateTime(row["DATUM_UHRADY"])
            );
            
            return (T)Convert.ChangeType(orderStatus, typeof(T));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM stavy";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var orderStatus = new OrderStatus(
                    Convert.ToInt32(row["ID_STAV"]),
                    Convert.ToDateTime(row["DATUM_PRIJETI"]),
                    Convert.ToDateTime(row["DATUM_UHRADY"])
                );
                OrderStatuses.Add(orderStatus);
            }
        }

        public async Task Add(OrderStatus entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "DATUM_PRIJETI", entity.OrderDate },
                { "DATUM_UHRADY", entity.PaymentDate }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addstavy", parameters);
        }

        public async Task Update(OrderStatus entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_STAV", entity.Id },
                { "DATUM_PRIJETI", entity.OrderDate },
                { "DATUM_UHRADY", entity.PaymentDate }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatestavy", parameters);
        }

        public async Task Delete(OrderStatus entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_STAV", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletestavy", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            var dataTable = new DataTable();
            
            dataTable.Columns.Add("ID_STAV", typeof(Int32));
            dataTable.Columns.Add("DATUM_PRIJETI", typeof(DateTime));
            dataTable.Columns.Add("DATUM_UHRADY", typeof(DateTime));
            
            foreach (var orderStatus in OrderStatuses)
            {
                dataTable.Rows.Add(orderStatus.Id, orderStatus.OrderDate, orderStatus.PaymentDate);
            }
            
            return dataTable;
        }
    }
}