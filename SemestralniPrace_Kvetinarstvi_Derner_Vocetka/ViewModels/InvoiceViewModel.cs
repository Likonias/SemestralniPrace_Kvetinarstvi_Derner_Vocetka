using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        public InvoiceViewModel(INavigationService navigationService, InvoiceStore invoiceStore)
        {
            NavigationService = navigationService;
            this.invoiceStore = invoiceStore;
        }

        public INavigationService NavigationService { get; }
        public InvoiceStore invoiceStore { get; }
    }
}
