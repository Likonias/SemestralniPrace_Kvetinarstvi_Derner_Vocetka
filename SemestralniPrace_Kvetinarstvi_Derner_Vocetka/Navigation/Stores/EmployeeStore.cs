using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class EmployeeStore
    {

        private Action employeeChanged;
        private Customer employee;
        public Customer Employee { get { return employee; } set { employee = value; OnCurrentCustomerChanged(); } }

        public void OnCurrentCustomerChanged()
        {
            employeeChanged?.Invoke();
        }

    }
}
