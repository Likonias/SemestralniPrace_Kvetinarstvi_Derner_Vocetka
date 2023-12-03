using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private OrderStore orderStore;

        public OrderViewModel(INavigationService navigationService, OrderStore orderStore)
        {
            this.navigationService = navigationService;
            this.orderStore = orderStore;
        }
    }
}
