using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class CustomerStore
    {

        private Action customerChanged;
        private Customer customer;
        public Customer Customer { get { return customer; } set { customer = value; OnCurrentCustomerChanged(); } }

        public void OnCurrentCustomerChanged()
        {
            customerChanged?.Invoke();
        }

    }
}
