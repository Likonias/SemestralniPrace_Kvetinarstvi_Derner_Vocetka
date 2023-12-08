using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class OrderStore
    {

        public int Id { get; set; }
        public int IdAccount { get; set; }
        public bool IsCustomer { get; set; }
        public OrderStore()
        {
           

        }
    }
}
