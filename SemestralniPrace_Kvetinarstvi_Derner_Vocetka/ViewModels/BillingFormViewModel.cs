using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class BillingFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; }
        public RelayCommand BtnOk { get; }
        private readonly INavigationService closeModalNavigationService;
        private readonly BillingStore billingStore;
        private readonly INavigationService? openBillingViewModel;
        public string errorMessage;

        public BillingFormViewModel(INavigationService closeModalNavigationService, BillingStore billingStore, INavigationService? openBillingViewModel)
        {
            this.closeModalNavigationService = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
            BtnOk = new RelayCommand(Ok);
            this.billingStore = billingStore;
            this.openBillingViewModel = openBillingViewModel;
        }

        private void Cancel()
        {
            closeModalNavigationService.Navigate();
        }

        private async void Ok()
        {
            if (CheckBilling())
            {
                var billingRepository = new BillingRepository();

                if (billingStore.Billing == null)
                {
                    var billing = new Billing(0, billingStore.BillingType, billingStore.Note);
                    await billingRepository.Add(billing);
                }
                else
                {
                    var billing = new Billing(billingStore.Billing.Id, billingStore.BillingType, billingStore.Note);
                    await billingRepository.Update(billing);
                }

                closeModalNavigationService.Navigate();
                openBillingViewModel?.Navigate();
            }
            else
            {
                ErrorMessage = "Adding failed!";
            }
        }

        private bool CheckBilling()
        {
            return billingStore.BillingType != BillingTypeEnum.Cash;
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
    }
}
