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
        private bool isAdmin;
        public AddressRepository(bool isAdmin)
        {
            Addresses = new ObservableCollection<Address>();
            dbUtil = new OracleDbUtil();
            this.isAdmin = isAdmin;
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
            await dbUtil.ExecuteStoredProcedureAsync("AddData.addadresy", parameters);

        }

        public async Task Delete(int id)
        {
            string command = "";

            if (isAdmin)
            {
                command = "DELETEADRESY_ADMIN";
            }
            else
            {
                command = "DELETEADRESY";
            }
            var parameters = new Dictionary<string, object>
            {
                { "ID_ADRESA", id }
            };
            await dbUtil.ExecuteStoredProcedureAsync(command, parameters);
        }

        

        public async Task GetAll()
        {
            Addresses.Clear();
            string command = "";

            if (isAdmin)
            {
                command = "GET_ALL_ADRESY_ADMIN";
            }
            else
            {
                command = "GET_ALL_ADDRESSES";
            }
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var address = new Address(
                    Convert.ToInt32(row["ID_ADRESA"]),
                    row["ULICE"].ToString(),
                    row["CISLO_POPISNE"].ToString(),
                    row["MESTO"].ToString(),
                    row["PSC"].ToString(),
                    row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["ZAMESTNANCI_ID_ZAMESTNANEC"]) : (int?)null,
                    row["ZAKAZNICI_ID_ZAKAZNIK"] != DBNull.Value ? Convert.ToInt32(row["ZAKAZNICI_ID_ZAKAZNIK"]) : (int?)null,
                    row["DRUHY_ADRES_ID_DRUH_ADRESY"] != DBNull.Value ? Convert.ToInt32(row["DRUHY_ADRES_ID_DRUH_ADRESY"]) : (int?)null
                );
                Addresses.Add(address);
            }

        }

        public async Task<Address> GetById(int id)
        {

            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            string command = "GET_ADDRESS_BY_ID";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);

            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            var address = new Address(
                Convert.ToInt32(row["ID_ADRESA"]),
                row["ULICE"].ToString(),
                row["CISLO_POPISNE"].ToString(),
                row["MESTO"].ToString(),
                row["PSC"].ToString(),
                row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["ZAMESTNANCI_ID_ZAMESTNANEC"]) : (int?)null,
                row["ZAKAZNICI_ID_ZAKAZNIK"] != DBNull.Value ? Convert.ToInt32(row["ZAKAZNICI_ID_ZAKAZNIK"]) : (int?)null,
                row["DRUHY_ADRES_ID_DRUH_ADRESY"] != DBNull.Value ? Convert.ToInt32(row["DRUHY_ADRES_ID_DRUH_ADRESY"]) : (int?)null
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
            string command = "";

            if (isAdmin)
            {
                command = "UPDATEADRESY_ADMIN";
            }
            else
            {
                command = "UPDATEADRESY";
            }
            await dbUtil.ExecuteStoredProcedureAsync(command, parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Street", typeof(string));
            dataTable.Columns.Add("StreetNumber", typeof(string));
            dataTable.Columns.Add("City", typeof(string));
            dataTable.Columns.Add("Zip", typeof(string));
            //dataTable.Columns.Add("AddressType", typeof(AddressType));

            foreach (var address in Addresses)
            {
                
                DataRow row = dataTable.NewRow();
                row["Id"] = address.Id;
                row["Street"] = address.Street;
                row["StreetNumber"] = address.StreetNumber;
                row["City"] = address.City;
                row["Zip"] = address.Zip;
                //row["AddressType"] = address.AddressTypeId. ?? null; 

                dataTable.Rows.Add(row);
            }

            return dataTable;
        }

        public List<Address> GetAddresses()
        {
            Task.Run(async () => await GetAll()).Wait();
            var addresses = Addresses.ToList();

            return addresses;
        }

        public Task Delete(Address entity)
        {
            throw new NotImplementedException();
        }
    }

    
}
