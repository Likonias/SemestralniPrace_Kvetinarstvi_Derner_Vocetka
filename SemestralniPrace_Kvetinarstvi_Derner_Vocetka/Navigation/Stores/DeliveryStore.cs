using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class DeliveryStore
    {
        private Action deliveryChanged;
        private Models.Entities.Delivery delivery;

        public Models.Entities.Delivery Delivery
        {
            get { return delivery; }
            set
            {
                delivery = value;
                OnCurrentDeliveryChanged();
            }
        }

        public void OnCurrentDeliveryChanged()
        {
            deliveryChanged?.Invoke();
        }
    }
}
