using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Interfaces;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories
{
    public class AddressRepository : IRepository<Address>
    {

        public ObservableCollection<Address> Addresses { get; set; }
        private OracleDbUtil dbUtil;

        public AddressRepository() 
        {
            Addresses = new ObservableCollection<Address>();
            dbUtil = new OracleDbUtil();
        }

        public async Task Add(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ULICE", entity.Street },
                { "CISLO_POPISNE", entity.StreetNumber },
                { "MESTO", entity.City },
                { "PSC", entity.Zip },
                { "ZAMESTNANCI_ID_ZAMESTNANEC", entity.EmployeeId },
                { "ZAKAZNICI_ID_ZAKAZNIK", entity.CustomerId },
                { "DRUHY_ADRES_ID_DRUH_ADRESY", entity.AddressTypeId },
            };
            await dbUtil.ExecuteStoredProcedureAsync("addadresy", parameters);

        }

        public async Task Delete(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ADRESA", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteadresy", parameters);
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM Adresy";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var address = new Address(
                    Convert.ToInt32(row["ID_ADRESA"]),
                    row["ULICE"].ToString(),
                    row["CISLO_POPISNE"].ToString(),
                    row["MESTO"].ToString(),
                    row["PSC"].ToString(),
                    row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : (int?)null,
                    row["ZAKAZNICI_ID_ZAKAZNIK"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : (int?)null,
                    row["DRUHY_ADRES_ID_DRUH_ADRESY"] != DBNull.Value ? Convert.ToInt32(row["AddressTypeId"]) : (int?)null
                );
                Addresses.Add(address);
            }

        }

        public async Task<Address> GetById(int id)
        {
            string command = $"SELECT * FROM Addresses WHERE Id = {id}";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var address = new Address(
                Convert.ToInt32(row["Id"]),
                row["Street"].ToString(),
                row["StreetNumber"].ToString(),
                row["City"].ToString(),
                row["Zip"].ToString(),
                row["EmployeeId"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : (int?)null,
                row["CustomerId"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : (int?)null,
                row["AddressType"] != DBNull.Value ? Convert.ToInt32(row["AddressTypeId"]) : (int?)null
            );

            return address;
        }

        public async Task Update(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ADRESA", entity.Id },
                { "ULICE", entity.Street },
                { "CISLO_POPISNE", entity.StreetNumber },
                { "MESTO", entity.City },
                { "PSC", entity.Zip },
                { "ZAMESTNANCI_ID_ZAMESTNANEC", entity.EmployeeId },
                { "ZAKAZNICI_ID_ZAKAZNIK", entity.CustomerId },
                { "DRUHY_ADRES_ID_DRUH_ADRESY", entity.AddressTypeId },
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateAddress", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Street", typeof(string));
            dataTable.Columns.Add("StreetNumber", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Zip", typeof(string));
            dataTable.Columns.Add("AddressType", typeof(AddressType));

            foreach (var address in Addresses)
            {
                
                DataRow row = dataTable.NewRow();
                row["Street"] = address.Street;
                row["StreetNumber"] = address.StreetNumber;
                row["City"] = address.City;
                row["Zip"] = address.Zip;
                row["AddressType"] = address.AddressTypeId ?? null; 

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
