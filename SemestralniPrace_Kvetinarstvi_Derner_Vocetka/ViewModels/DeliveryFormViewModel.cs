using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; }
        public RelayCommand BtnOk { get; }

        private readonly DeliveryStore deliveryStore;
        private readonly INavigationService closeModalNavigationService;
        private readonly INavigationService openDeliveryViewModel;
        private Delivery delivery;
        private readonly DeliveryRepository deliveryRepository;

        public DeliveryFormViewModel(INavigationService closeModalNavigationService, DeliveryStore deliveryStore, INavigationService openDeliveryViewModel)
        {
            this.closeModalNavigationService = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            this.deliveryStore = deliveryStore;
            this.openDeliveryViewModel = openDeliveryViewModel;
            deliveryRepository = new DeliveryRepository();
            delivery = deliveryStore.Delivery;
            if (delivery != null) { InitializeDelivery(); }
        }

        private void Cancel()
        {
            closeModalNavigationService.Navigate();
        }

        private async void Ok()
        {
            DeliveryRepository deliveryRepository = new DeliveryRepository();

            if (deliveryStore.Delivery == null)
            {
                delivery = new Delivery(0, DateTime.MinValue, null, DeliveryMethodEnum.D, 0, null);
                await deliveryRepository.Add(delivery);
            }
            else
            {
                delivery = new Delivery(
                    deliveryStore.Delivery.IdDeliveryMethod,
                    deliveryStore.Delivery.WarehouseReleaseDate,
                    deliveryStore.Delivery.IdOrder,
                    deliveryStore.Delivery.Method,
                    deliveryStore.Delivery.IdDelivery,
                    deliveryStore.Delivery.TransportCompany
                );
                await deliveryRepository.Update(delivery);
            }

            closeModalNavigationService.Navigate();
            openDeliveryViewModel.Navigate();
        }

        private void InitializeDelivery()
        {
            // Populate your ViewModel properties from the Delivery model
            // Example: _transportCompany = delivery.TransportCompany;
        }

        // Add your ViewModel properties for data binding
        // Example: private string _transportCompany;
        // public string TransportCompany { get => _transportCompany; set { _transportCompany = value; OnPropertyChanged(nameof(TransportCompany)); } }
    }
}
