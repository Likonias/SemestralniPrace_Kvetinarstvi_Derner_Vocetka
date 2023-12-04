using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressTypeViewModel : ViewModelBase
    {
        public AddressTypeViewModel(INavigationService navigationService, AddressTypeStore addressTypeStore)
        {
            NavigationService = navigationService;
            AddressTypeStore = addressTypeStore;
        }

        public INavigationService NavigationService { get; }
        public AddressTypeStore AddressTypeStore { get; }
    }
}
