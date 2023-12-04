using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InPersonPickupViewModel : ViewModelBase
    {
        public InPersonPickupViewModel(INavigationService navigationService, InPersonPickupStore inPersonPickupStore)
        {
            NavigationService = navigationService;
            InPersonPickupStore = inPersonPickupStore;
        }

        public INavigationService NavigationService { get; }
        public InPersonPickupStore InPersonPickupStore { get; }
    }
}
