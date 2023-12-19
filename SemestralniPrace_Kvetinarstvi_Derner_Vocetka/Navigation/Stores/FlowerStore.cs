using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class FlowerStore
    {
        private Action flowerChanged;
        private Flower flower;

        public Flower Flower
        {
            get { return flower; }
            set
            {
                flower = value;
                OnCurrentFlowerChanged();
            }
        }

        public void OnCurrentFlowerChanged()
        {
            flowerChanged?.Invoke();
        }
    }
}
