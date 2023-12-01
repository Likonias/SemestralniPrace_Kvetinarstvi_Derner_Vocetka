using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class OtherGoods : Goods
    {
        public OtherGoods(int idGoods, string name, double price, byte type, int warehouse, byte[]? image, int idOtherGoods, string countryOfOrigin, DateTime expirationDate) : base(idGoods, name, price, type, warehouse, image)
        {
            IdOtherGoods = idOtherGoods;
            CountryOfOrigin = countryOfOrigin;
            ExpirationDate = expirationDate;
        }
        public int IdOtherGoods { get; set; }
        public string? CountryOfOrigin { get; set; }
        public DateTime? ExpirationDate { get; set; }

    }
}
