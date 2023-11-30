using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores
{
    public class AccountStore
    {

        private Account currentAccount;

        public Account CurrentAccount
        {
            get => currentAccount;
            set
            {
                currentAccount = value;
                CurrentAccountChanged?.Invoke();
            }
        }

        public event Action CurrentAccountChanged;

        public void Logout()
        {
            CurrentAccount = null;
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public bool IsLoggedOut => CurrentAccount == null;

    }
}
