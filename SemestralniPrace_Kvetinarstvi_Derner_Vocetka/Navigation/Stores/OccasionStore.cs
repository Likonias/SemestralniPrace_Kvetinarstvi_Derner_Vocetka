using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class OccasionStore
    {
        private Action occasionChanged;
        private Occasion occasion;

        public Occasion Occasion
        {
            get { return occasion; }
            set
            {
                occasion = value;
                OnCurrentOccasionChanged();
            }
        }

        public void OnCurrentOccasionChanged()
        {
            occasionChanged?.Invoke();
        }
    }
}
