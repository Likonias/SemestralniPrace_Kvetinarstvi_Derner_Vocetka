using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressFormViewModel : ViewModelBase
    {
        public RelayCommand BtnCancel { get; private set; }
        public string Street { get; set; }
        private INavigationService closeNavSer;
        public AddressFormViewModel(INavigationService closeModalNavigationService)
        {
            closeNavSer = closeModalNavigationService;
            BtnCancel = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            closeNavSer.Navigate();
        }

    }
}
