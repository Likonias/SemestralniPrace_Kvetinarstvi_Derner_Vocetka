using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InPersonPickupFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public RelayCommand BtnOk { get; private set; }
        private readonly AccountStore accountStore;
        public string errorMessage;

        private INavigationService closeNavSer;
        private InPersonPickup inPersonPickup;
        private InPersonPickupStore inPersonPickupStore;
        private INavigationService openInPersonPickupViewModel;

        public InPersonPickupFormViewModel(INavigationService closeModalNavigationService, InPersonPickupStore inPersonPickupStore, INavigationService? openInPersonPickupViewModel)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            inPersonPickup = inPersonPickupStore.InPersonPickup;
            this.inPersonPickupStore = inPersonPickupStore;
            this.openInPersonPickupViewModel = openInPersonPickupViewModel;
            if (inPersonPickup != null) { InitializeInPersonPickup(); }
            _time = string.Empty;
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

        private async void Ok()
        {
            if (CheckInPersonPickup())
            {
                InPersonPickupRepository inPersonPickupRepository = new InPersonPickupRepository();

                if (inPersonPickupStore.InPersonPickup == null)
                {
                    inPersonPickup = new InPersonPickup(0, WarehouseReleaseDate, OrderId, DeliveryMethodEnum.O, IdPickup, Time);
                    await inPersonPickupRepository.Add(inPersonPickup);
                }
                else
                {
                    inPersonPickup = new InPersonPickup(inPersonPickupStore.InPersonPickup.IdDeliveryMethod, WarehouseReleaseDate, OrderId, DeliveryMethodEnum.O, IdPickup, Time);
                    await inPersonPickupRepository.Update(inPersonPickup);
                }

                closeNavSer.Navigate();
                openInPersonPickupViewModel.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckInPersonPickup()
        {
            return WarehouseReleaseDate != DateTime.MinValue || OrderId != null || IdPickup != 0 || !string.IsNullOrEmpty(Time);
        }

        private void InitializeInPersonPickup()
        {
            _warehouseReleaseDate = inPersonPickup.WarehouseReleaseDate;
            _orderId = inPersonPickup.IdOrder;
            _idPickup = inPersonPickup.IdPickup;
            _time = inPersonPickup.Time;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        private DateTime _warehouseReleaseDate;
        public DateTime WarehouseReleaseDate
        {
            get => _warehouseReleaseDate;
            set
            {
                _warehouseReleaseDate = value;
                OnPropertyChanged(nameof(WarehouseReleaseDate));
            }
        }

        private int? _orderId;
        public int? OrderId
        {
            get => _orderId;
            set
            {
                _orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }

        private int _idPickup;
        public int IdPickup
        {
            get => _idPickup;
            set
            {
                _idPickup = value;
                OnPropertyChanged(nameof(IdPickup));
            }
        }

        private string _time;
        public string Time
        {
            get => _time;
            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }
    }
}
