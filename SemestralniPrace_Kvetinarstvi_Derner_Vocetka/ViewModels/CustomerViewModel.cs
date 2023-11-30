using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {

        private CustomerStore customerStore;

        public CustomerViewModel(CustomerStore customerStore)
        {
            this.customerStore = customerStore;
        }
    }
}
