using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities
{
    public class Delivery : DeliveryMethod
    {
        public Delivery(int idDeliveryMethod, DateTime warehouseReleaseDate, int? idOrder, DeliveryMethodEnum method, int idDelivery, string? transportCompany) : base(idDeliveryMethod, warehouseReleaseDate, idOrder, method)
        {
            IdDelivery = idDelivery;
            TransportCompany = transportCompany;
        }

        public int IdDelivery { get; set; }
        public string? TransportCompany { get; set; }

    }
}
