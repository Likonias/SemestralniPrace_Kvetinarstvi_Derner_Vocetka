using System.Threading.Tasks;
using System.Windows.Input;
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
        private OracleDbUtil dbUtil; // Instance OracleDbUtil, díky které jsme schopni komunikovat s databází
        private AccountStore accountStore;
        public LoginViewModel(AccountStore accountStore, INavigationService closeModalNavigationService)
        {
            dbUtil = new OracleDbUtil();
            this.accountStore = accountStore;
            LoginCommand = new RelayCommand(Login);
            CancelCommand = new RelayCommand(Close);
            this.closeNavigationService = closeModalNavigationService;
            
        }

        private async void Login()
        {
            if(await dbUtil.ExecuteStoredValidateLoginFunctionAsync("validateLogin", Email, Password))
            {
                accountStore.CurrentAccount = await GetUser(Email);
                closeNavigationService.Navigate();
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