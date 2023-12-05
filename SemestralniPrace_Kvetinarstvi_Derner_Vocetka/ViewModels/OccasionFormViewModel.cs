using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OccasionFormViewModel : ViewModelBase
    {
        public OccasionFormViewModel(OccasionStore occasionStore, INavigationService navigationService)
        {
            OccasionStore = occasionStore;
            NavigationService = navigationService;
        }

        public OccasionStore OccasionStore { get; }
        public INavigationService NavigationService { get; }
    }
}
