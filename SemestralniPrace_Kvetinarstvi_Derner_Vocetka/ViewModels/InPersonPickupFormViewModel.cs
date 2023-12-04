using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InPersonPickupFormViewModel : ViewModelBase
    {
        public InPersonPickupFormViewModel(InPersonPickupStore inPersonPickupStore, INavigationService navigationService)
        {
            InPersonPickupStore = inPersonPickupStore;
            NavigationService = navigationService;
        }

        public InPersonPickupStore InPersonPickupStore { get; }
        public INavigationService NavigationService { get; }
    }
}
