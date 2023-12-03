using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class EmployeeFormViewModel : ViewModelBase
    {
        private EmployeeStore employeeStore;
        private INavigationService navigationService;

        public EmployeeFormViewModel(EmployeeStore employeeStore, INavigationService navigationService)
        {
            this.employeeStore = employeeStore;
            this.navigationService = navigationService;
        }
    }
}
