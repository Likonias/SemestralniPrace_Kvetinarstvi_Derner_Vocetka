using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class LowStockLogViewModel : ViewModelBase
    {
        private AccountStore accountStore;
        private INavigationService closeNavigationService;
        private LowStockLogChecker lowStockLogChecker;
        public RelayCommand BtnCancel { get; private set; }
        private DataTable tableData;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }


        public LowStockLogViewModel(AccountStore accountStore, INavigationService closeNavigationService, LowStockLogChecker lowStockLogChecker)
        {
            this.accountStore = accountStore;
            this.closeNavigationService = closeNavigationService;
            this.lowStockLogChecker = lowStockLogChecker;
            TableData = lowStockLogChecker.TableData;
            BtnCancel = new RelayCommand(Cancel);
        }

        private void Cancel()
        {
            closeNavigationService.Navigate();
        }
    }
}
