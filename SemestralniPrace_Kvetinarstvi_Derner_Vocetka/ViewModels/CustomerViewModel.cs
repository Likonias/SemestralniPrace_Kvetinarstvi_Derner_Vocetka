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

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class CustomerViewModel : ViewModelBase
    {

        private CustomerStore customerStore;
        private OracleDbUtil dbUtil;
        private DataTable tableData;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        private bool isAnonymous;
        public bool IsAnonymous { get { return isAnonymous; } set { isAnonymous = value; OnPropertyChanged(nameof(IsAnonymous)); } }
        private DataRowView selectedItem;
        public DataRowView SelectedItem 
        { 
            get { return selectedItem; } 
            set 
            {
                selectedItem = value;
                if (SelectedItem.Row[1].ToString() == "anonymous")
                {
                    IsAnonymous = false;
                }
                else
                {
                    IsAnonymous = true;
                }
                 OnPropertyChanged(nameof(SelectedItem)); 
            } 
        }
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private INavigationService createCustomerForm;
        private CustomerRepository customerRepository;
        public CustomerViewModel(INavigationService createCustomerForm, CustomerStore customerStore, AccountStore accountStore)
        {
            this.customerStore = customerStore;
            this.createCustomerForm = createCustomerForm;
            customerRepository = new CustomerRepository(accountStore.CurrentAccount.EmployeePosition == Models.Enums.EmployeePositionEnum.ADMIN);
            BtnAdd = new RelayCommand(BtnAddPresseed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            InitializeTableData();
            IsAnonymous = true;
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                await customerRepository.Delete(int.Parse(SelectedItem.Row[0].ToString()));
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                customerStore.Customer = await customerRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createCustomerForm.Navigate();
            }



        }
        private void BtnAddPresseed()
        {

            customerStore.Customer = null;
            createCustomerForm.Navigate();

        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await customerRepository.ConvertToDataTable();

        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private string searchQuery;
        public string SearchQuery
        {
            get { return searchQuery; }
            set
            {
                searchQuery = value;
                FilterTableData();
                OnPropertyChanged(nameof(SearchQuery));
            }
        }

        private void FilterTableData()
        {
            if (string.IsNullOrEmpty(SearchQuery))
            {
                InitializeTableData(); // Reset to the original data if search query is empty
                return;
            }

            DataView dv = tableData.DefaultView;
            dv.RowFilter = $"FirstName LIKE '%{SearchQuery}%' OR " +
                   $"LastName LIKE '%{SearchQuery}%' OR " +
                   $"Email LIKE '%{SearchQuery}%' OR " +
                   $"Tel LIKE '%{SearchQuery}%'";
            TableData = dv.ToTable();
        }
    }
}
