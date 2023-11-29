using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class AddressViewModel : ViewModelBase
    {

        private OracleDbUtil dbUtil;
        private DataTable tableData;

        public RelayCommand BtnAdd { get; }

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        private INavigationService createAddressForm;

        public AddressViewModel(INavigationService createAddressForm)
        {
            this.createAddressForm = createAddressForm;
            BtnAdd = new RelayCommand(BtnAddPresseed);
            this.dbUtil = dbUtil;
            this.tableData = tableData;
            InitializeTableData();
        }

        private void BtnAddPresseed()
        {

            createAddressForm.Navigate();
            
        }

        private async Task InitializeTableData()
        {
            
            AddressRepository addressRepository = new AddressRepository();
            TableData = await addressRepository.ConvertToDataTable();

        }
    }
}
