using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryMethodFormViewModel : ViewModelBase
    {
        public DeliveryMethodFormViewModel(DeliveryStore deliveryStore, INavigationService navigationService)
        {
            DeliveryStore = deliveryStore;
            NavigationService = navigationService;
        }

        public DeliveryStore DeliveryStore { get; }
        public INavigationService NavigationService { get; }
    }
}
