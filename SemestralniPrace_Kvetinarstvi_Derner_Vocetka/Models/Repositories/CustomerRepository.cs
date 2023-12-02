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
    public class CustomerRepository : IRepository<Customer>
    {
        public ObservableCollection<Customer> Customers { get; set; }
        private OracleDbUtil dbUtil;
        
        public CustomerRepository()
        {
            Customers = new ObservableCollection<Customer>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Customer> GetById(Int32 id)
        {
            string command = $"SELECT * FROM zakaznici WHERE ID_ZAKAZNIK = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);
            var row = dataTable.Rows[0];
            var customer = new Customer(
                Convert.ToInt32(row["ID_ZAKAZNIK"]),
                row["JMENO"].ToString(),
                row["PRIJMENI"].ToString(),
                row["EMAIL"].ToString(),
                row["TELEFON"].ToString(),
                row["HESLO"].ToString()
            );
            return customer;
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM zakaznici";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var customer = new Customer(
                    Convert.ToInt32(row["ID_ZAKAZNIK"]),
                    row["JMENO"].ToString(),
                    row["PRIJMENI"].ToString(),
                    row["EMAIL"].ToString(),
                    row["TELEFON"].ToString(),
                    row["HESLO"].ToString()
                );
                Customers.Add(customer);
            }
        }

        public async Task Add(Customer entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "JMENO", entity.FirstName },
                { "PRIJMENI", entity.LastName },
                { "EMAIL", entity.Email },
                { "TELEFON", entity.Tel },
                { "HESLO", entity.Password }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addzakaznici", parameters);
        }

        public async Task Update(Customer entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAKAZNIK", entity.Id },
                { "JMENO", entity.FirstName },
                { "PRIJMENI", entity.LastName },
                { "EMAIL", entity.Email },
                { "TELEFON", entity.Tel },
                { "HESLO", entity.Password }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatezakaznici", parameters);
        }

        public async Task Delete(Customer entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAKAZNIK", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletezakaznici", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("Tel");
            dataTable.Columns.Add("Password");
            
            foreach (var customer in Customers)
            {
                dataTable.Rows.Add(
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.Tel,
                    customer.Password
                );
            }

            return dataTable;
        }

        public List<Customer> GetCustomers()
        {
            Task.Run(async () => await GetAll()).Wait();
            var customers = Customers.ToList();

            return customers;
        }

        public List<Customer> GetCustomersForAdmin()
        {
            // TODO: Implement logic to get customers for admin
            // For now, let's return all customers as a placeholder
            return Customers.ToList();
        }

        public List<Customer> GetCustomersForMajitel()
        {
            // TODO: Implement logic to get customers for majitel
            // For now, let's return an empty list as a placeholder
            return new List<Customer>();
        }

        public List<Customer> GetCustomersForProdavac()
        {
            // TODO: Implement logic to get customers for prodavac
            // For now, let's return an empty list as a placeholder
            return new List<Customer>();
        }
    }    
}



