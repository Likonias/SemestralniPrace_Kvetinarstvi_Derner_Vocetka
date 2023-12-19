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
    public class EmployeeRepository
    {
        public ObservableCollection<Employee> Employees { get; set; }
        private OracleDbUtil dbUtil;
        
        public EmployeeRepository()
        {
            Employees = new ObservableCollection<Employee>();
            dbUtil = new OracleDbUtil();
        }
        
        public async Task<Employee> GetById(int id)
        {
            string command = "GET_ZAMESTNANEC_BY_ID";
            var parameters = new Dictionary<string, object>
            {
                { "p_id", id },
            };
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command, parameters);
            if (dataTable.Rows.Count == 0)
                return null;
            var row = dataTable.Rows[0];
            var employee = new Employee(
                Convert.ToInt32(row["ID_ZAMESTNANEC"]),
                row["JMENO"].ToString(),
                row["PRIJMENI"].ToString(),
                Convert.ToInt32(row["MZDA"]),
                row["EMAIL"].ToString(),
                row["TELEFON"].ToString(),
                row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["ZAMESTNANCI_ID_ZAMESTNANEC"]) : (int?)null,
                row["HESLO"].ToString(),
                (EmployeePositionEnum)Enum.Parse(typeof(EmployeePositionEnum), row["POZICE"].ToString())

            );
            return employee;
        }

        public async Task GetAll()
        {
            Employees.Clear();
            string command = "GET_ALL_ZAMESTNANCI";
            DataTable dataTable = await dbUtil.ExecuteCommandAsync(command);

            foreach (DataRow row in dataTable.Rows)
            {
                var employee = new Employee(
                    Convert.ToInt32(row["ID_ZAMESTNANEC"]),
                    row["JMENO"].ToString(),
                    row["PRIJMENI"].ToString(),
                    Convert.ToDouble(row["MZDA"]),
                    row["EMAIL"].ToString(),
                    row["TELEFON"].ToString(),
                    row["ZAMESTNANCI_ID_ZAMESTNANEC"] != DBNull.Value ? Convert.ToInt32(row["ZAMESTNANCI_ID_ZAMESTNANEC"]) : (int?)null,
                    row["HESLO"].ToString(),
                    (EmployeePositionEnum)Enum.Parse(typeof(EmployeePositionEnum), row["POZICE"].ToString())
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
                { "ZAMESTNANCI_ID_ZAMESTNANEC", entity.IdSupervisor},
                { "HESLO", entity.Password },
                { "POZICE", entity.Position.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("AddData.addzamestnanec", parameters);
        }

        public async Task Update(int id, string firstName, string lastName, double wage, string email, string tel, int? idSupervisor, EmployeePositionEnum position)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAMESTNANEC", id },
                { "JMENO", firstName },
                { "PRIJMENI", lastName },
                {"MZDA", wage},
                { "EMAIL", email },
                { "TELEFON", tel },
                { "ZAMESTNANCI_ID_ZAMESTNANEC", idSupervisor},
                { "POZICE", position.ToString() }
            };
            await dbUtil.ExecuteStoredProcedureAsync("UpdateData.updatezamestnanec", parameters);
        }

        public async Task Delete(int id)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ID_ZAMESTNANEC", id }
            };
            await dbUtil.ExecuteStoredProcedureAsync("DeleteData.deletezamestnanec", parameters);
        }

        public async Task<DataTable> ConvertToDataTable()
        {
            await GetAll();
            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("FirstName");
            dataTable.Columns.Add("LastName");
            dataTable.Columns.Add("Wage");
            dataTable.Columns.Add("Email");
            dataTable.Columns.Add("Tel");
            dataTable.Columns.Add("Supervisor");
            dataTable.Columns.Add("Position");
            
            foreach (var employee in Employees)
            {

                string supervisorEmail = string.Empty;

                if (employee.IdSupervisor != null)
                {
                    var supervisor = Employees.FirstOrDefault(e => e.Id == employee.IdSupervisor);
                    supervisorEmail = supervisor?.Email ?? "Supervisor Not Found";
                }

                dataTable.Rows.Add(
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Wage,
                    employee.Email,
                    employee.Tel,
                    supervisorEmail,
                    employee.Position
                );
            }
            
            return dataTable;
        }

        public List<Employee> GetEmployees()
        {
            Task.Run(async () => await GetAll()).Wait(); // Wait for the asynchronous GetAll to complete
            return Employees.ToList();
        }

    }
}