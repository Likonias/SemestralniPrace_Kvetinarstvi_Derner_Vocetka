using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryMethodFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }

        public ObservableCollection<DeliveryMethodEnum> DeliveryMethodComboBoxItems { get; set; }

        private INavigationService closeNavService;
        private Models.Entities.DeliveryMethod deliveryMethod;
        private DeliveryMethodStore deliveryMethodStore;
        private INavigationService openDeliveryMethodViewModel;

        public DeliveryMethodFormViewModel(
            INavigationService closeModalNavigationService,
            DeliveryMethodStore deliveryMethodStore,
            INavigationService? openDeliveryMethodViewModel)
        {
            closeNavService = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            deliveryMethod = deliveryMethodStore.DeliveryMethod;
            this.deliveryMethodStore = deliveryMethodStore;
            this.openDeliveryMethodViewModel = openDeliveryMethodViewModel;

            DeliveryMethodComboBoxItems = new ObservableCollection<DeliveryMethodEnum>(
                Enum.GetValues(typeof(DeliveryMethodEnum)) as DeliveryMethodEnum[]
            );
        }

        private void Cancel()
        {
            closeNavService.Navigate();
        }

        private void Ok()
        {
            // TODO: Implement your logic for saving or updating the DeliveryMethod
            // Use the properties from the ViewModel, e.g., DeliveryMethod.IdDeliveryMethod, DeliveryMethod.WarehouseReleaseDate, etc.

            closeNavService.Navigate();
            openDeliveryMethodViewModel.Navigate();
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
