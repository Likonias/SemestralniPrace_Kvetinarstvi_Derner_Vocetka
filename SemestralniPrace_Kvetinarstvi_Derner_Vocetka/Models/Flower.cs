using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Flower : Goods
    {
        public FlowerState State { get; set; }
        public int Age { get; set; }

        public Flower(FlowerState state, int age)
        {
            State = state;
            Age = age;
        }
    }
}
