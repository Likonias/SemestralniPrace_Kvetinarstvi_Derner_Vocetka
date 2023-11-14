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
        private readonly Navigation navigation;
        public ViewModelBase CurrentViewModel => navigation.CurrentViewModel;

        public MainWindowViewModel(Navigation navigation)
        {
            this.navigation = navigation;
            this.navigation.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
