﻿using System.Threading.Tasks;
using System.Windows.Input;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string email;
        private string password;
        private string errorMessage;
        public string ErrorMessage { get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }
        private INavigationService closeNavigationService;
        private INavigationService accountNavigationService;
        private OracleDbUtil dbUtil; // Instance OracleDbUtil, díky které jsme schopni komunikovat s databází
        private AccountStore accountStore;
        private INavigationService lowStockLog;
        private LowStockLogChecker lowStockLogChecker;
        public LoginViewModel(AccountStore accountStore, INavigationService closeModalNavigationService, INavigationService accountNavigationService, INavigationService lowStockLog, LowStockLogChecker lowStockLogChecker)
        {
            dbUtil = new OracleDbUtil();
            this.accountStore = accountStore;
            LoginCommand = new RelayCommand(Login);
            CancelCommand = new RelayCommand(Close);
            this.closeNavigationService = closeModalNavigationService;
            this.accountNavigationService = accountNavigationService;
            this.lowStockLog = lowStockLog;
            this.lowStockLogChecker = lowStockLogChecker;
        }

        private async void Login()
        {
            if (await dbUtil.ExecuteStoredValidateLoginFunctionAsync("validateLogin", Email, PasswordHash.PasswordHashing(Password)))
            {
                accountStore.CurrentAccount = await GetUser(Email);
                closeNavigationService.Navigate();
                accountNavigationService.Navigate();
                if(accountStore.CurrentAccount.EmployeePosition != null && lowStockLogChecker.TableData.Rows.Count > 0)
                {
                    lowStockLog.Navigate();
                }
            }
            else
            {
                ErrorMessage = "Login failed!";
            }

        }

        private async Task<Account> GetUser(string email)
        {
            Account acc = await dbUtil.ExecuteGetAccountFunctionAsync("getUserByEmail", email);
            return acc;
        }

        private void Close()
        {
            closeNavigationService.Navigate();
        }

    }
}