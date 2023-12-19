using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class DeliveryMethodStore
    {
        private Action deliveryMethodChanged;
        private Models.Entities.DeliveryMethod deliveryMethod;

        public Models.Entities.DeliveryMethod DeliveryMethod
        {
            get { return deliveryMethod; }
            set
            {
                deliveryMethod = value;
                OnCurrentDeliveryMethodChanged();
            }
        }

        public void OnCurrentDeliveryMethodChanged()
        {
            deliveryMethodChanged?.Invoke();
        }
    }
}
