using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class OrderStatusStore
    {
        private Action orderStatusChanged;
        private OrderStatus orderStatus;

        public OrderStatus OrderStatus
        {
            get { return orderStatus; }
            set
            {
                orderStatus = value;
                OnCurrentOrderStatusChanged();
            }
        }

        public void OnCurrentOrderStatusChanged()
        {
            orderStatusChanged?.Invoke();
        }
    }
}
