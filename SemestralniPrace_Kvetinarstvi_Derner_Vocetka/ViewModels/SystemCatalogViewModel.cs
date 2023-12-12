using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Components;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class SystemCatalogViewModel : ViewModelBase
    {
        //todo redo it
        private OracleDbUtil dbUtil;
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
        private string commandText;
        public ObservableCollection<string> SystemCatalogComboBoxItems { get; set; }
        private string selectedSystemCatalogComboBoxItem;
        public string SelectedSystemCatalogComboBoxItem
        {
            get => selectedSystemCatalogComboBoxItem;
            set
            {
                selectedSystemCatalogComboBoxItem = value;
                OnPropertyChanged(nameof(SelectedSystemCatalogComboBoxItem));
                InitializeTableData();
            }
        }
        public SystemCatalogViewModel()
        {
            dbUtil = new OracleDbUtil();
            SystemCatalogComboBoxItems = new ObservableCollection<string>();
            InitializeTableData();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            foreach(SystemCatalogEnum val in Enum.GetValues(typeof(SystemCatalogEnum)))
            {
                SystemCatalogComboBoxItems.Add(val.ToString());
            }
        }

        private async void InitializeTableData()
        {
            switch (SelectedSystemCatalogComboBoxItem)
            {
                case "Tables":
                    commandText = "SELECT object_name, object_type FROM user_objects WHERE object_type = 'TABLE'";
                    break;
                case "Views":
                    commandText = "SELECT object_name, object_type FROM user_objects WHERE object_type = 'VIEW'";
                    break;
                case "Tables_Views":
                    commandText = "SELECT object_name, object_type FROM user_objects WHERE object_type IN ('TABLE', 'VIEW')";
                    break;
                case "Procedures":
                    commandText = "SELECT object_name, procedure_name, object_type FROM user_procedures";
                    break;
                case "All_Objects":
                    commandText = "SELECT object_name, object_type FROM user_objects";
                    break;
                case "All_Tab_Columns":
                    commandText = "SELECT table_name, column_name, data_type, data_length FROM all_tab_columns";
                    break;
                case "All_Ind_Columns":
                    commandText = "SELECT index_name, table_name, column_name FROM all_ind_columns";
                    break;
                case "Statistics":
                    commandText = "SELECT table_name, num_rows, tablespace_name FROM user_tables";
                    break;
                case "Complete_Ind_Columns":
                    commandText = "SELECT * FROM all_ind_columns";
                    break;
                case "User_Tables":
                    commandText = "SELECT * FROM user_tables";
                    break;
                default:
                    commandText = "SELECT object_name, object_type FROM user_objects WHERE object_type IN ('TABLE', 'VIEW')";
                    break;
            }

            TableData = await dbUtil.ExecuteQueryAsync(commandText, null);

        }

        private enum SystemCatalogEnum
        {
            Tables,
            Views,
            Tables_Views,
            Procedures,
            All_Objects,
            All_Tab_Columns,
            All_Ind_Columns,
            Statistics,
            Complete_Ind_Columns,
            User_Tables
            
        }

    }
}
