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

        public NavigationBarViewModel(NavigationService<LoginViewModel> loginNavigationService, NavigationService<RegisterViewModel> registerNavigationService)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(loginNavigationService);
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(registerNavigationService);
            //TODO implement account
            //NavigateAccountCommand =
            //NavigateViewCommand = 

        }
    }
}
