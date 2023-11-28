using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class AddressTypeRepository : IRepository<AddressType>
    {
        public ObservableCollection<AddressType> AddressType { get; set; }
        private OracleDbUtil dbUtil;

        public AddressTypeRepository()
        {
            AddressType = new ObservableCollection<AddressType>();
            dbUtil = new OracleDbUtil();
        }

        public async Task<T> GetById(Int32 id)
        {
            string command = $"SELECT * FROM druh_adresy WHERE ID_DRUH_ADRESY = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);
            var row = dataTable.Rows[0];
            var addressType = new AddressType(
                Convert.ToInt32(row["ID_DRUH_ADRESY"]),
                (AddressTypeEnum)Enum.Parse(typeof(AddressTypeEnum), row["DRUH_ADRESY"].ToString())
            );
            return (T)Convert.ChangeType(addressType, typeof(T));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM druh_adresy";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var addressType = new AddressType(
                    Convert.ToInt32(row["ID_DRUH_ADRESY"]),
                    (AddressTypeEnum)Enum.Parse(typeof(AddressTypeEnum), row["DRUH_ADRESY"].ToString())
                );
                AddressType.Add(addressType);
            }
        }

        public async Task Add(AddressType entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "DRUH_ADRESY", entity.AddressType.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("adddruh_adresy", parameters);
        }
        public async Task Update(AddressType entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DRUH_ADRESY", entity.Id },
                { "DRUH_ADRESY", entity.AddressType.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatedruh_adresy", parameters);
        }

        public async Task Delete(AddressType entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DRUH_ADRESY", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletedruh_adresy", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("AddressType", typeof(AddressTypeEnum));
            
            foreach (var addressType in AddressType)
            {
                DataRow row = dataTable.NewRow();
                row["AddressType"] = addressType.AddressType.toString();
                dataTable.Rows.Add(row);
            }
            
            return dataTable;
        }
    }
}
