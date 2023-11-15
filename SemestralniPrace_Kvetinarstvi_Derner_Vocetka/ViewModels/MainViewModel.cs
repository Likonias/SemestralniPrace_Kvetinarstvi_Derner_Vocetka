using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class MainViewModel : ViewModelBase
    {

        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateRegisterCommand { get; }

        public MainViewModel(Navigation navigation)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigation, () => new LoginViewModel(navigation));
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(navigation, () => new RegisterViewModel(navigation));
        }
    }
}
