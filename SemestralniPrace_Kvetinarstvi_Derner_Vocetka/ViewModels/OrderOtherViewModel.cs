using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OrderOtherViewModel : ViewModelBase
    {
        public RelayCommand BtnClose { get; }
        public OrderOtherViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            BtnClose = new RelayCommand(BtnCloseClicked);
        }

        private void BtnCloseClicked()
        {
            NavigationService.Navigate();
        }

        public INavigationService NavigationService { get; }
    }
}
