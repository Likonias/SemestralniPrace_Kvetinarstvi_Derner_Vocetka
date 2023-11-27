using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Flower : Goods
    {
        public Flower(int idGoods, string name, double price, byte type, int warehouse, byte[]? image, int idFlower, FlowerStateEnum state, int age) : base(idGoods, name, price, type, warehouse, image)
        {
            IdFlower = idFlower;
            State = state;
            Age = age;
        }

        public int IdFlower { get; set; }
        public FlowerStateEnum? State { get; set; }
        public int Age { get; set; }
        

    }
}
