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
        private AccountStore accountStore;
        private string name;
        public string Name { get { return name; } set { name = value; OnPropertyChanged(Name); } }
        private string email;
        public string Email { get { return email; } set { email = value; OnPropertyChanged(Email); } }
        public AccountViewModel(AccountStore accountStore)
        {
            this.accountStore = accountStore;
            Name = accountStore.CurrentAccount.FirstName + " " + accountStore.CurrentAccount.LastName;
            Email = accountStore.CurrentAccount.Email;
        }
    }
}
