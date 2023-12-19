using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand LoadLoginCommand { get; private set; }
        public MainWindowViewModel()
        {
            //LoadLoginCommand = new RelayCommand(ShowLoginView);
        }

        private void ShowLoginView()
        {
            LoginView loginView = new LoginView();
            
        }
    }
}
