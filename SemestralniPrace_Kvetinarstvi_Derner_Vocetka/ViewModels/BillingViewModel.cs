using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class BillingViewModel : ViewModelBase
    {
        public BillingViewModel(INavigationService navigationService, BillingStore billingStore)
        {
            NavigationService = navigationService;
            BillingStore = billingStore;
        }

        public INavigationService NavigationService { get; }
        public BillingStore BillingStore { get; }
    }
}
