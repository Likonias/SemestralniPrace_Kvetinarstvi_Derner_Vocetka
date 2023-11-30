using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation
{
    public class NavigationStore
    {
        public event Action CurrentViewModelChanged;
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel { 
            
            get => currentViewModel; 
            set 
            { 
                currentViewModel?.Dispose();
                currentViewModel = value; 
                OnCurrentViewModelChanged(); 
            } 
        }

        private void OnCurrentViewModelChanged()
        {
            //event handler for a change of the view
            CurrentViewModelChanged?.Invoke();
        }
    }
}
