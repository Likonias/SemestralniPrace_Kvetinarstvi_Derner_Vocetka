using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil; // Initialize this in the constructor

        public ObservableCollection<string> TableNames { get; set; } // Bind this to ComboBox.ItemsSource

        private DataTable selectedTableData;
        public DataTable SelectedTableData
        {
            get => selectedTableData;
            set
            {
                selectedTableData = value;
                OnPropertyChanged(nameof(SelectedTableData)); // Notify UI of changes
            }
        }

        private string selectedTableName;
        public string SelectedTableName
        {
            get => selectedTableName;
            set
            {
                selectedTableName = value;
                LoadSelectedTableData(); // Fetch data when table selection changes
                OnPropertyChanged(nameof(SelectedTableName));
            }
        }

        public ICommand NavigateLoginCommand { get; }
        public ICommand NavigateRegisterCommand { get; }
       
        public MainViewModel(Navigation navigation)
        {
            NavigateLoginCommand = new NavigateCommand<LoginViewModel>(navigation, () => new LoginViewModel(navigation));
            NavigateRegisterCommand = new NavigateCommand<RegisterViewModel>(navigation, () => new RegisterViewModel(navigation));

            dbUtil = new OracleDbUtil(); // Initialize the database utility

            // Populate the ComboBox with table names on initialization
            TableNames = new ObservableCollection<string>(GetTableNamesFromDatabase());

            LoadSelectedTableData();
        }

        private List<string> GetTableNamesFromDatabase()
        {
            // Implement a method in OracleDbUtil to fetch table names from the database
            // Example:
            // List<string> tableNames = dbUtil.GetTableNames();
            // return tableNames;
            return new List<string>(); // Placeholder, replace this with actual table names
        }

        private void LoadSelectedTableData()
        {
            if (!string.IsNullOrEmpty(SelectedTableName))
            {
                // Fetch data for the selected table using OracleDbUtil
                // Example:
                // SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM {SelectedTableName}");
                SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM zakaznici");
                
            }
            SelectedTableData = dbUtil.ExecuteQuery($"SELECT * FROM zakaznici");
        }

    }

}
