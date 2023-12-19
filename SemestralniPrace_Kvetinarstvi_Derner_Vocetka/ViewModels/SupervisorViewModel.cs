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
    public class SupervisorViewModel : ViewModelBase
    {
        public RelayCommand BtnClose { get; }
        private DataTable tableData;
        private OracleDbUtil dbUtil;
        private EmployeeStore employeeStore;
        public INavigationService CloseNavigationService { get; }
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public SupervisorViewModel(EmployeeStore employeeStore, INavigationService closeNavigationService) {
            this.employeeStore = employeeStore;
            CloseNavigationService = closeNavigationService;
            BtnClose = new RelayCommand(BtnCloseClicked);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            
            DataTable dt = await dbUtil.GetHierarchyAsync("GetEmployeeHierarchy", employeeStore.Id);
            TableData = new DataTable();

            TableData.Columns.Add("Jmeno");
            TableData.Columns.Add("Prijmeni");
            TableData.Columns.Add("Email");
            TableData.Columns.Add("Telefon");

            foreach (DataRow row in dt.Rows)
            {
                TableData.Rows.Add(
                        row["JMENO"].ToString(),
                        row["PRIJMENI"].ToString(),
                        row["EMAIL"].ToString(),
                        row["TELEFON"].ToString()
                    );
            }
        }

        private void BtnCloseClicked()
        {
            CloseNavigationService.Navigate();
        }
    }
}
