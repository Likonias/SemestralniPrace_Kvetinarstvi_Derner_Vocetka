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
        private readonly NavigationStore navigationStore;
        private readonly ModalNavigationStore modalNavigationStore;
        public ViewModelBase CurrentViewModel => navigationStore.CurrentViewModel;
        public ViewModelBase CurrentModalViewModel => modalNavigationStore.CurrentViewModel;
        public bool IsOpen => modalNavigationStore.IsOpen;
        public MainWindowViewModel(NavigationStore navigation, ModalNavigationStore modalNavigationStore)
        {
            this.navigationStore = navigation;
            this.navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            this.modalNavigationStore = modalNavigationStore;
            this.modalNavigationStore.CurrentViewModelChanged += OnCurrentModalViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            //Update of the viewModel when the change occours
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentModalViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentModalViewModel));
            OnPropertyChanged(nameof(IsOpen));
        }
    }
}
