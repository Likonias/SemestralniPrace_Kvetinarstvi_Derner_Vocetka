using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        public RelayCommand RegisterCommand { get; }
        public ICommand CancelCommand { get; }
        public RegisterViewModel(INavigationService createMainNavigationService)
        {
            RegisterCommand = new RelayCommand(Register);
            CancelCommand = new NavigateCommand<MainViewModel>(createMainNavigationService);
        }

        public void Register()
        {
            //TODO založení nového uživatele, basic privileges a přidání do databáze
        }
    }
}
