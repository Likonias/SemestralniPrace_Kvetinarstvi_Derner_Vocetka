using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        public async Task<AddressType> GetById(Int32 id)
        {
            string command = $"SELECT * FROM druh_adresy WHERE ID_DRUH_ADRESY = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);
            var row = dataTable.Rows[0];
            var addressType = new AddressType(
                Convert.ToInt32(row["ID_DRUH_ADRESY"]),
                (AddressTypeEnum)Enum.Parse(typeof(AddressTypeEnum), row["DRUH_ADRESY"].ToString())
            );
            return (AddressType)Convert.ChangeType(addressType, typeof(AddressType));
        }

        public async Task GetAll()
        {
            AddressType.Clear();
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
                { "DRUH_ADRESY", entity.addressType.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("adddruh_adresy", parameters);
        }
        public async Task Update(AddressType entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DRUH_ADRESY", entity.Id },
                { "DRUH_ADRESY", entity.addressType.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatedruh_adresy", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_DRUH_ADRESY", id }
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
                row["AddressType"] = addressType.addressType;
                dataTable.Rows.Add(row);
            }
            
            return dataTable;
        }

        public List<AddressType> GetAddressTypes()
        {
            Task.Run(async () => await GetAll()).Wait();
            var addressTypes = AddressType.ToList();

            return addressTypes;
        }
    }
}
