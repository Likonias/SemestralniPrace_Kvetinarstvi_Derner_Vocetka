using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class InvoiceFormViewModel : ViewModelBase
    {
        public InvoiceFormViewModel(InvoiceStore invoiceStore, INavigationService navigationService)
        {
            InvoiceStore = invoiceStore;
            NavigationService = navigationService;
        }

        public InvoiceStore InvoiceStore { get; }
        public INavigationService NavigationService { get; }
    }
}
