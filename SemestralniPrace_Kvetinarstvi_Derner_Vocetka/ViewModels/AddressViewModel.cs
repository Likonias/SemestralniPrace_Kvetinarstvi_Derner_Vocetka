using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
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
        public DataRowView SelectedItem { get; set; }
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
        private AddressRepository addressRepository;
        public AddressViewModel(INavigationService createAddressForm, AddressStore addressStore)
        {
            this.createAddressForm = createAddressForm;
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            this.addressStore = addressStore;
            tableData = new DataTable();
            addressRepository = new AddressRepository();
            InitializeTableData();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await addressRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                addressStore.Address = await addressRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createAddressForm.Navigate();
            }



        }
        private void BtnAddPresseed()
        {

            addressStore.Address = null;
            createAddressForm.Navigate();
            
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await addressRepository.ConvertToDataTable();
            
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
