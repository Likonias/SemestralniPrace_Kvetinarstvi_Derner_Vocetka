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
        
    }


}
