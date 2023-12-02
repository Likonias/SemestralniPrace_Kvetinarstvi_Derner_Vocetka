using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderFormViewModel : ViewModelBase
    {
        private OrderStore orderStore;
        private INavigationService navigationService;

        public OrderFormViewModel(OrderStore orderStore, INavigationService navigationService)
        {
            this.orderStore = orderStore;
            this.navigationService = navigationService;
        }
    }
}
