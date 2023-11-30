using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class LayoutNavigationService<TViewModel> : INavigationService where TViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        
        private readonly Func<TViewModel> createViewModel;
        private readonly Func<NavigationBarViewModel> createNavigationBarViewModel;

        public LayoutNavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel, Func<NavigationBarViewModel> createNavigationBarViewModel)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
            this.createNavigationBarViewModel = createNavigationBarViewModel;
        }

        public void Navigate()
        {
            navigationStore.CurrentViewModel = new LayoutViewModel(createNavigationBarViewModel(), createViewModel());
        }

        
    }
}
