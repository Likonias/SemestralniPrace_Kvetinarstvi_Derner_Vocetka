using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class LoginCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly LoginViewModel viewModel;
        private readonly ParameterNavigationService<Account, AccountViewModel> navigationService;

        public LoginCommand(LoginViewModel viewModel, ParameterNavigationService<Account, AccountViewModel> navigationService)
        {
            this.viewModel = viewModel;
            this.navigationService = navigationService;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
