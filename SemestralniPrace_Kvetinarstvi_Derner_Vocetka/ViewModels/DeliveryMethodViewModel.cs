using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class DeliveryMethodViewModel : ViewModelBase
    {
        public DeliveryMethodViewModel(INavigationService navigationService, DeliveryStore deliveryStore)
        {
            NavigationService = navigationService;
            DeliveryStore = deliveryStore;
        }

        public INavigationService NavigationService { get; }
        public DeliveryStore DeliveryStore { get; }
    }
}
