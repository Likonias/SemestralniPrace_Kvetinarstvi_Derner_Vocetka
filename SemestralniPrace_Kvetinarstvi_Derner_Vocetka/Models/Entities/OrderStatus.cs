using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly? PaymentDate { get; set; }

        public OrderStatus(int id, DateOnly orderDate, DateOnly? paymentDate)
        {
            Id = id;
            OrderDate = orderDate;
            PaymentDate = paymentDate;
        }
    }
}
