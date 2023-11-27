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
        public DataTable DataTable { get; set; }
        private OracleDbUtil dbUtil;

        public AddressRepository() 
        {
            Addresses = new ObservableCollection<Address>();
            dbUtil = new OracleDbUtil();
            DataTable = new DataTable();
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
                { "DRUHY_ADRES_ID_DRUH_ADRESY", entity.AddressType },
            };
            await dbUtil.ExecuteStoredProcedureAsync("addadresy", parameters);

        }

        public async Task Delete(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "AddressId", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deleteAddress", parameters);
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM Adresy"; // Modify this query based on your database schema
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            //foreach (DataRow row in dataTable.Rows)
            //{
            //    var address = new Address(
            //        Convert.ToInt32(row["Id"]),
            //        row["Street"].ToString(),
            //        row["StreetNumber"].ToString(),
            //        row["City"].ToString(),
            //        row["Zip"].ToString(),
            //        row["EmployeeId"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : (int?)null,
            //        row["CustomerId"] != DBNull.Value ? Convert.ToInt32(row["CustomerId"]) : (int?)null,
            //        row["AddressType"] != DBNull.Value ? (AddressType?)Enum.Parse(typeof(AddressType), row["AddressType"].ToString()) : null
            //    );
            //    Addresses.Add(address);
            //}

            DataTable = dataTable;

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
                row["AddressType"] != DBNull.Value ? (AddressType?)Enum.Parse(typeof(AddressType), row["AddressType"].ToString()) : null
            );

            return address;
        }

        public async Task Update(Address entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "AddressId", entity.Id }, // Assuming AddressId is the identifier for update
                { "ULICE", entity.Street },
                { "CISLO_POPISNE", entity.StreetNumber },
                { "MESTO", entity.City },
                { "PSC", entity.Zip },
                { "ZAMESTNANCI_ID_ZAMESTNANEC", entity.EmployeeId },
                { "ZAKAZNICI_ID_ZAKAZNIK", entity.CustomerId },
                { "DRUHY_ADRES_ID_DRUH_ADRESY", entity.AddressType },
            };
            await dbUtil.ExecuteStoredProcedureAsync("updateAddress", parameters);
        }

        public DataTable ConvertToDataTable()
        {
            GetAll();
            DataTable dataTable = new DataTable();

            // Adding columns to the DataTable based on Address properties
            dataTable.Columns.Add("Street", typeof(string));
            dataTable.Columns.Add("StreetNumber", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Zip", typeof(string));
            dataTable.Columns.Add("EmployeeId", typeof(int));
            dataTable.Columns.Add("CustomerId", typeof(int));
            dataTable.Columns.Add("AddressType", typeof(AddressType));

            foreach (var address in Addresses)
            {
                // Creating a new row in the DataTable and assigning values
                DataRow row = dataTable.NewRow();
                row["Street"] = address.Street;
                row["StreetNumber"] = address.StreetNumber;
                row["City"] = address.City;
                row["Zip"] = address.Zip;
                row["EmployeeId"] = address.EmployeeId ?? 0; // Handle nullable types
                row["CustomerId"] = address.CustomerId ?? 0; // Handle nullable types
                row["AddressType"] = address.AddressType ?? null; // Handle nullable types

                // Adding the populated row to the DataTable
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
