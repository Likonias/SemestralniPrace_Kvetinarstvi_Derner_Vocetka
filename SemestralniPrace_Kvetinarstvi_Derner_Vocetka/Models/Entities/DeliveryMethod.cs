using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities
{
    public class DeliveryMethod
    {
        public int IdDeliveryMethod { get; set; }
        public DateTime WarehouseReleaseDate { get; set; }
        public int? IdOrder { get; set; }
        public DeliveryMethodEnum Method { get; set; }

        public DeliveryMethod(int idDeliveryMethod, DateTime warehouseReleaseDate, int? idOrder, DeliveryMethodEnum method)
        {
            IdDeliveryMethod = idDeliveryMethod;
            WarehouseReleaseDate = warehouseReleaseDate;
            IdOrder = idOrder;
            Method = method;
        }
    }
}
