using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
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
        private readonly NavigationStore navigation;
        public ViewModelBase CurrentViewModel => navigation.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigation)
        {
            this.navigation = navigation;
            this.navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }
        
        private void OnCurrentViewModelChanged()
        {
            //Update of the viewModel when the change occours
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
