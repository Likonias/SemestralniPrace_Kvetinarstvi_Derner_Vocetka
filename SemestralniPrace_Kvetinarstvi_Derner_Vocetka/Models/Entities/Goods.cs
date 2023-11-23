﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models
{
    public class Goods
    {
        public string Name { get; set; }
        public double Price { get; set; }

        public byte[] Image { get; set; }

        public Goods(string name, double price, byte[] image)
        {
            Name = name;
            Price = price;
            Image = image;
        }
    }
}