using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly Navigation navigation;
        private readonly Func<TViewModel> createViewModel;

        public NavigateCommand(Navigation navigation, Func<TViewModel> createViewModel)
        {
            this.navigation = navigation;
            this.createViewModel = createViewModel;
        }
        public override void Execute(object parameter)
        {
            navigation.CurrentViewModel = createViewModel();
        }
    }
}
