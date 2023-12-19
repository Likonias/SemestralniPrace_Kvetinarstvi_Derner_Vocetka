using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class AddressTypeStore
    {
        private AddressType addressType;

        public AddressType AddressType
        {
            get { return addressType; }
            set
            {
                addressType = value;
                OnCurrentAddressTypeChanged();
            }
        }

        public event Action AddressTypeChanged;

        public void OnCurrentAddressTypeChanged()
        {
            AddressTypeChanged?.Invoke();
        }
    }
}
