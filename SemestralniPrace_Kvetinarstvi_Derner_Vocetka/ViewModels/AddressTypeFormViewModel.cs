using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressTypeFormViewModel : ViewModelBase
    {
        public AddressTypeFormViewModel(AddressTypeStore addressTypeStore, INavigationService navigationService)
        {
            AddressTypeStore = addressTypeStore;
            NavigationService = navigationService;
        }

        public AddressTypeStore AddressTypeStore { get; }
        public INavigationService NavigationService { get; }
    }
}
