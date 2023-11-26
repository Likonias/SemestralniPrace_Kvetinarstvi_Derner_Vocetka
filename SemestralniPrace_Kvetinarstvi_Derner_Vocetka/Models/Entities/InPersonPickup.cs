using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities
{
    public class InPersonPickup : DeliveryMethod
    {
        public InPersonPickup(int idDeliveryMethod, DateTime warehouseReleaseDate, int? idOrder, DeliveryMethodEnum method, int idPickup, string time) : base(idDeliveryMethod, warehouseReleaseDate, idOrder, method)
        {
            IdPickup = idPickup;
            Time = time;
        }

        public int IdPickup { get; set; }
        public string Time { get; set; }

    }
}
