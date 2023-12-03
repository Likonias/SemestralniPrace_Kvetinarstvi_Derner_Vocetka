using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;

         public MainViewModel(INavigationService createLoginNavigationService)
        {
            
            dbUtil = new OracleDbUtil(); // Initialize the database utility

        }
        

    }

}
