using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderStatusFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        private readonly AccountStore accountStore;
        public string errorMessage;

        private INavigationService closeNavSer;
        private OrderStatus orderStatus;
        private OrderStatusStore orderStatusStore;
        private INavigationService openOrderStatusViewModel;

        public OrderStatusFormViewModel(INavigationService closeModalNavigationService, OrderStatusStore orderStatusStore, INavigationService? openOrderStatusViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            orderStatus = orderStatusStore.OrderStatus;
            this.orderStatusStore = orderStatusStore;
            this.openOrderStatusViewModel = openOrderStatusViewModel;
            if (orderStatus != null) { InitializeOrderStatus(); }
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckOrderStatus())
            {
                OrderStatusRepository orderStatusRepository = new OrderStatusRepository();

                if (orderStatusStore.OrderStatus == null)
                {
                    orderStatus = new OrderStatus(0, OrderDate, PaymentDate);
                    await orderStatusRepository.Add(orderStatus);
                }
                else
                {
                    orderStatus = new OrderStatus(orderStatusStore.OrderStatus.Id, OrderDate, PaymentDate);
                    await orderStatusRepository.Update(orderStatus);
                }

                closeNavSer.Navigate();
                openOrderStatusViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckOrderStatus()
        {
            // Add your validation logic here
            return true;
        }

        private void InitializeOrderStatus()
        {
            _orderDate = orderStatus.OrderDate;
            _paymentDate = orderStatus.PaymentDate;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private DateTime _orderDate;
        public DateTime OrderDate
        {
            get => _orderDate;
            set
            {
                _orderDate = value;
                OnPropertyChanged(nameof(OrderDate));
            }
        }

        private DateTime? _paymentDate;
        public DateTime? PaymentDate
        {
            get => _paymentDate;
            set
            {
                _paymentDate = value;
                OnPropertyChanged(nameof(PaymentDate));
            }
        }
    }
}
