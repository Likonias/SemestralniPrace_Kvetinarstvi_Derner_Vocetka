using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Goods
    {
        public int IdGoods { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public byte Type { get; set; }
        public int Warehouse { get; set; }
        public byte[]? Image { get; set; }

        public Goods(int idGoods, string name, double price, byte type, int warehouse, byte[]? image)
        {
            IdGoods = idGoods;
            Name = name;
            Price = price;
            Type = type;
            Warehouse = warehouse;
            Image = image;
        }
    }
}
