using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class EmployeeViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private EmployeeStore employeeStore;

        public EmployeeViewModel(INavigationService navigationService, EmployeeStore employeeStore)
        {
            this.navigationService = navigationService;
            this.employeeStore = employeeStore;
        }
    }
}
