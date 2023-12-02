using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OtherGoodsFormViewModel : ViewModelBase
    {
        private OtherGoodsStore otherGoodsStore;
        private INavigationService navigationService;

        public OtherGoodsFormViewModel(OtherGoodsStore otherGoodsStore, INavigationService navigationService)
        {
            this.otherGoodsStore = otherGoodsStore;
            this.navigationService = navigationService;
        }
    }
}
