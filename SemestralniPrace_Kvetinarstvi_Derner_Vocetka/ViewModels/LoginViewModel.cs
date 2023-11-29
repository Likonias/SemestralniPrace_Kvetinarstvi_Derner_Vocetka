using System.Windows.Input;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string email;
        private string password;
        private string errorMessage;

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

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }
        
        public RelayCommand LoginCommand { get; }
        public RelayCommand CancelCommand { get; }
        private INavigationService closeNavigationService;
        private OracleDbUtil dbUtil; // Instance OracleDbUtil, díky které jsme schopni komunikovat s databází
        public LoginViewModel(AccountStore accountStore, INavigationService closeModalNavigationService)
        {
            dbUtil = new OracleDbUtil();
            LoginCommand = new RelayCommand(Login);
            CancelCommand = new RelayCommand(Close);
            this.closeNavigationService = closeModalNavigationService;
        }

        private void Login()
        {
            //TODO logika loginu, načtení z databáze, nalezení dle emailu a následné checknutí hesla přes PasswordHash (nejdříve se heslo musí hashnout)
        }

        private void Close()
        {
            closeNavigationService.Navigate();
        }

    }
}