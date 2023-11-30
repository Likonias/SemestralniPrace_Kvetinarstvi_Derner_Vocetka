using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
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
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }

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
        private AddressStore addressStore;

        public AddressViewModel(INavigationService createAddressForm, AddressStore addressStore)
        {
            this.createAddressForm = createAddressForm;
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            this.addressStore = addressStore;
            //TODO delete later
            this.addressStore.Address = new Models.Address(1, "st", "5", "Pce", "53002", null, null, new AddressType(1, Models.Enums.AddressTypeEnum.Billing));
            InitializeTableData();
        }

        private void BtnDeletePressed()
        {
            throw new NotImplementedException();
        }

        private void BtnEditPressed()
        {
            createAddressForm.Navigate();
        }

        private void BtnAddPresseed()
        {

            addressStore.Address = null;
            createAddressForm.Navigate();
            
        }

        private async Task InitializeTableData()
        {
            
            AddressRepository addressRepository = new AddressRepository();
            TableData = await addressRepository.ConvertToDataTable();

        }
    }
}
