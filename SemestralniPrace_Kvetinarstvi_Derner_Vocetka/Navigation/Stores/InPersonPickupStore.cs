using System;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class InPersonPickupStore
    {
        private Action inPersonPickupChanged;
        private InPersonPickup inPersonPickup;

        public InPersonPickup InPersonPickup
        {
            get { return inPersonPickup; }
            set
            {
                inPersonPickup = value;
                OnCurrentInPersonPickupChanged();
            }
        }

        public void OnCurrentInPersonPickupChanged()
        {
            inPersonPickupChanged?.Invoke();
        }
    }
}
