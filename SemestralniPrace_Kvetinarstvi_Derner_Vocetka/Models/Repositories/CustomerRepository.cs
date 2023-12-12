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
    public class CustomerRepository
    {
        public ObservableCollection<Customer> Customers { get; set; }
        private OracleDbUtil dbUtil;
        
        public CustomerRepository()
        {
            Customers = new ObservableCollection<Customer>();
            dbUtil = new OracleDbUtil();
        }
        private bool isAdmin;
        public CustomerRepository(bool isAdmin)
        {
            Customers = new ObservableCollection<Customer>();
            dbUtil = new OracleDbUtil();
            this.isAdmin = isAdmin;
        }

        public async Task<Customer> GetById(Int32 id)
        {
            string command = "GET_ZAKAZNIK_BY_ID";
            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);
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
            Customers.Clear();

            string command = "";

            if (isAdmin)
            {
                command = "GET_ALL_ZAKAZNICI_ADMIN";
            }
            else
            {
                command = "GET_ALL_ZAKAZNICI";
            }

            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

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

        public async Task Update(int id, string firstName, string lastName, string email, string tel)
        {
            string command = "";

            if (isAdmin)
            {
                command = "UPDATEZAKAZNICI_ADMIN";
            }
            else
            {
                command = "UPDATEZAKAZNICI";
            }
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAKAZNIK", id },
                { "JMENO", firstName },
                { "PRIJMENI", lastName },
                { "EMAIL", email },
                { "TELEFON", tel },
            };
            await dbUtil.ExecuteStoredProcedureAsync(command, parameters);
        }

        public async Task Delete(int id)
        {
            string command = "";

            if (isAdmin)
            {
                command = "DELETEZAKAZNICI_ADMIN";
            }
            else
            {
                command = "DELETEZAKAZNICI";
            }
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAKAZNIK", id }
            };
            await dbUtil.ExecuteStoredProcedureAsync(command, parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("Tel");
            dataTable.Columns.Add("Password");
            
            foreach (var customer in Customers)
            {
                dataTable.Rows.Add(
                    customer.Id,
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
    }    
}



