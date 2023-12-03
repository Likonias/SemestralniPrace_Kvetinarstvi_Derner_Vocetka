using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OtherGoodsViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private OtherGoodsStore otherGoodsStore;

        public OtherGoodsViewModel(INavigationService navigationService, OtherGoodsStore otherGoodsStore)
        {
            this.navigationService = navigationService;
            this.otherGoodsStore = otherGoodsStore;
        }
    }
}
