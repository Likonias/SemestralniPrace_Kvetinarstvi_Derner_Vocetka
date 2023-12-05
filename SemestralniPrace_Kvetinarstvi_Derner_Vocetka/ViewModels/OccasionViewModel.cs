using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OccasionViewModel : ViewModelBase
    {
        public OccasionViewModel(INavigationService navigationService, OccasionStore occasionStore)
        {
            NavigationService = navigationService;
            OccasionStore = occasionStore;
        }

        public INavigationService NavigationService { get; }
        public OccasionStore OccasionStore { get; }
    }
}
