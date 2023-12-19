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

        private Account admin;

        private Account currentAccount;

        public Account CurrentAccount
        {
            get => currentAccount;
            set
            {
                currentAccount = value;
                if(currentAccount?.EmployeePosition == Models.Enums.EmployeePositionEnum.ADMIN) { admin = value; }
                CurrentAccountChanged?.Invoke();
            }
        }

        public event Action CurrentAccountChanged;

        public void Logout()
        {
            if (CurrentAccount.EmployeePosition == Models.Enums.EmployeePositionEnum.ADMIN) 
            { 
                isAdmin = false;
                admin = null;
            }
            if(admin != null)
            {
                CurrentAccount = admin;
            }
            else
            {
                CurrentAccount = null;
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public bool IsLoggedOut => CurrentAccount == null;
        private bool isAdmin;
        public bool IsCurrentAdmin { get => CurrentAccount?.EmployeePosition == Models.Enums.EmployeePositionEnum.ADMIN; }

    }
}
