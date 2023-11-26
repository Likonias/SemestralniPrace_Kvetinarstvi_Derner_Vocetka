using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Order
    {
        
        public int Id { get; set; }
        public double FinalPrice { get; set; }
        public OrderStatus IdStatus { get; set; }
        public Billing? IdBilling { get; set; }
        public Customer IdCustomer { get; set; }
        public Employee IdEmployee { get; set; }


    }
}
