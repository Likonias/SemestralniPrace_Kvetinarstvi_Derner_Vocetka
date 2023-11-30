using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AccountViewModel : ViewModelBase
    {
        //todo dodělat implementaci account v aplikaci pro jak zakaznika tak i zamestnance

        private AccountStore accountStore;

        public AccountViewModel(AccountStore accountStore)
        {
            this.accountStore = accountStore;
        }
    }
}
