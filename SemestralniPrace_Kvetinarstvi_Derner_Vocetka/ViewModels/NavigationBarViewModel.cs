using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class NavigationBarViewModel : ViewModelBase
    {

        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateRegisterCommand { get; }
        public ICommand NavigateAccountCommand { get; }
        public ICommand NavigateViewCommand { get; }

        //todo finish setting up an account
        private readonly AccountStore accountStore;

        public NavigationBarViewModel(AccountStore accountStore, INavigationService<LoginViewModel> loginNavigationService, INavigationService<RegisterViewModel> registerNavigationService)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(registerNavigationService);
            this.accountStore = accountStore;
            //TODO implement account
            //NavigateAccountCommand =
            //NavigateViewCommand = 

        }
    }
}
