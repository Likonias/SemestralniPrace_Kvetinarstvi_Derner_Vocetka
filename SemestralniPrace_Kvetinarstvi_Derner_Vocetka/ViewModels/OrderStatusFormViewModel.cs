using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderStatusFormViewModel : ViewModelBase
    {
        public OrderStatusFormViewModel(OrderStatusStore orderStatusStore, INavigationService navigationService)
        {
            OrderStatusStore = orderStatusStore;
            NavigationService = navigationService;
        }

        public OrderStatusStore OrderStatusStore { get; }
        public INavigationService NavigationService { get; }
    }
}
