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
    public class EmployeeRepository : IRepository<Employee>
    {
        public ObservableCollection<Employee> Employees { get; set; }
        private OracleDbUtil dbUtil;
        
        public EmployeeRepository()
        {
            Employees = new ObservableCollection<Employee>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Employee> GetById(Int32 id)
        {
            string command = $"SELECT * FROM zamestnanci WHERE ID_ZAMESTNANEC = {id}";
            var dataTable = await dbUtil.ExecuteQueryAsync(command);
            var row = dataTable.Rows[0];
            var employee = new Employee(
                Convert.ToInt32(row["ID_ZAMESTNANEC"]),
                row["JMENO"].ToString(),
                row["PRIJMENI"].ToString(),
                Convert.ToInt32(row["MZDA"]),
                row["EMAIL"].ToString(),
                row["TELEFON"].ToString(),
                row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : (int?)null,
                row["HESLO"].ToString(),
                (EmployeePositionEnum)(row["POZICE"] != DBNull.Value ? Enum.Parse(typeof(EmployeePositionEnum), row["EmployeePosition"].ToString()) : null)

            );
            return (Employee)Convert.ChangeType(employee, typeof(Employee));
        }

        public async Task GetAll()
        {
            string command = "SELECT * FROM zamestnanci";
            DataTable dataTable = await dbUtil.ExecuteQueryAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var employee = new Employee(
                    Convert.ToInt32(row["ID_ZAMESTNANEC"]),
                    row["JMENO"].ToString(),
                    row["PRIJMENI"].ToString(),
                    Convert.ToInt32(row["MZDA"]),
                    row["EMAIL"].ToString(),
                    row["TELEFON"].ToString(),
                    row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["EmployeeId"]) : (int?)null,
                    row["HESLO"].ToString(),
                    (EmployeePositionEnum)(row["POZICE"] != DBNull.Value ? Enum.Parse(typeof(EmployeePositionEnum), row["EmployeePosition"].ToString()) : null)

                );
                Employees.Add(employee);
            }
        }

        public async Task Add(Employee entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "JMENO", entity.FirstName },
                { "PRIJMENI", entity.LastName },
                {"MZDA", entity.Wage},
                { "EMAIL", entity.Email },
                { "TELEFON", entity.Tel },
                { "HESLO", entity.Password },
                { "POZICE", entity.Position }
            };
            await dbUtil.ExecuteStoredProcedureAsync("addzamestnanec", parameters);
        }

        public async Task Update(Employee entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAMESTNANEC", entity.Id },
                { "JMENO", entity.FirstName },
                { "PRIJMENI", entity.LastName },
                {"MZDA", entity.Wage},
                { "EMAIL", entity.Email },
                { "TELEFON", entity.Tel },
                { "HESLO", entity.Password },
                { "POZICE", entity.Position }
            };
            await dbUtil.ExecuteStoredProcedureAsync("updatezamestnanec", parameters);
        }

        public async Task Delete(Employee entity)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAMESTNANEC", entity.Id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("deletezamestnanec", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();
            
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("Wage");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("Tel");
            dataTable.Columns.Add("Password");
            dataTable.Columns.Add("Position");
            
            foreach (var employee in Employees)
            {
                dataTable.Rows.Add(
                    employee.FirstName,
                    employee.LastName,
                    employee.Wage,
                    employee.Email,
                    employee.Tel,
                    employee.Password,
                    employee.Position
                );
            }
            
            return dataTable;
        }
        
    }
}