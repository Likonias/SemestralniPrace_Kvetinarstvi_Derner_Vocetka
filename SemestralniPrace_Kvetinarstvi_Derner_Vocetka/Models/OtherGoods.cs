using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class OtherGoods : Goods
    {
        
        public string CountryOfOrigin { get; set; }
        public DateOnly ExpirationDate { get; set; }

        public OtherGoods(string name, double price, byte[] image, string countryOfOrigin, DateOnly expirationDate) : base(name, price, image)
        {
            CountryOfOrigin = countryOfOrigin;
            ExpirationDate = expirationDate;
        }

    }
}
