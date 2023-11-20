using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class LayoutNavigationService<TViewModel> : INavigationService<TViewModel> where TViewModel : ViewModelBase
    {
        private readonly NavigationStore navigationStore;
        
        private readonly Func<TViewModel> createViewModel;
        private readonly NavigationBarViewModel navigationBarViewModel;

        public LayoutNavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel, NavigationBarViewModel navigationBarViewModel)
        {
            this.navigationStore = navigationStore;
            this.createViewModel = createViewModel;
            this.navigationBarViewModel = navigationBarViewModel;
        }

        public void Navigate()
        {
            navigationStore.CurrentViewModel = new LayoutViewModel(navigationBarViewModel, createViewModel());
        }
    }
}
