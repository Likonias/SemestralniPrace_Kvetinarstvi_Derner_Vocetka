using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class BillingFormViewModel : ViewModelBase
    {
        public BillingFormViewModel(BillingStore billingStore, INavigationService navigationService)
        {
            BillingStore = billingStore;
            NavigationService = navigationService;
        }

        public BillingStore BillingStore { get; }
        public INavigationService NavigationService { get; }
    }
}
