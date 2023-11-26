using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class NavigationServiceManager
    {
        private Dictionary<Type, object> navigationServices = new Dictionary<Type, object>();

        public void RegisterNavigationService<TViewModel>(INavigationService<TViewModel> service) where TViewModel : ViewModelBase
        {
            navigationServices[typeof(TViewModel)] = service;
        }

        public INavigationService<TViewModel> GetNavigationService<TViewModel>() where TViewModel : ViewModelBase
        {
            if (navigationServices.TryGetValue(typeof(TViewModel), out object service))
            {
                return (INavigationService<TViewModel>)service;
            }

            return null; // or handle if service is not found
        }

        public IEnumerable<object> GetAllNavigationServices()
        {
            return navigationServices.Values;
        }

        public void ClearNavigationService()
        {
            navigationServices.Clear();
        }
    }

}
