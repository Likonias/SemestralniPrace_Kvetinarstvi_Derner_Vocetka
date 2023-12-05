using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderStatusViewModel : ViewModelBase
    {
        public OrderStatusViewModel(INavigationService navigationService, OrderStatusStore orderStatusStore)
        {
            NavigationService = navigationService;
            OrderStatusStore = orderStatusStore;
        }

        public INavigationService NavigationService { get; }
        public OrderStatusStore OrderStatusStore { get; }
    }
}
