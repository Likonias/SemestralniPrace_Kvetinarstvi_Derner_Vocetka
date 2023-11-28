using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Employee 
    { 
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Wage { get; set; }
        public string Email { get; set; }
        public string? Tel { get; set; }
        public int? IdSupervisor { get; set; }
        public string Password { get; set; }
        public EmployeePosition Position { get; set; }

        public Employee(int id, string firstName, string lastName, double wage, string email, string? tel, int? idSupervisor, string password, EmployeePosition position)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Wage = wage;
            Email = email;
            Tel = tel;
            IdSupervisor = idSupervisor;
            Password = password;
            Position = position;
        }
    }


}
