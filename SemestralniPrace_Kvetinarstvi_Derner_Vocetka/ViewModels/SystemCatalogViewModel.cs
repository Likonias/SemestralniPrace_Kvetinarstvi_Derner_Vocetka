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
                case "Table":
                    commandText = "GET_SYS_TABLE";
                    break;
                case "View":
                    commandText = "GET_SYS_VIEW";
                    break;
                case "Procedure":
                    commandText = "GET_SYS_PROCEDURE";
                    break;
                case "Trigger":
                    commandText = "GET_SYS_TRIGGER";
                    break;
                case "Sequence":
                    commandText = "GET_SYS_SEQUENCE";
                    break;
                case "Index":
                    commandText = "GET_SYS_INDEX";
                    break;
                case "Function":
                    commandText = "GET_SYS_FUNCTION";
                    break;
                case "Type":
                    commandText = "GET_SYS_TYPE";
                    break;
                case "Lob":
                    commandText = "GET_SYS_LOB";
                    break;
                case "Package":
                    commandText = "GET_SYS_PACKAGE";
                    break;
                case "Package_Body":
                    commandText = "GET_SYS_PACKAGE_BODY";
                    break;
                default:
                    commandText = "GET_SYS_ALL";
                    break;
            }
            DataTable dt = await dbUtil.ExecuteCommandAsync(commandText);
            TableData = new DataTable();
            TableData.Columns.Add("ObjectName");
            TableData.Columns.Add("ObjectType");

            foreach (DataRow row in dt.Rows)
            {
                TableData.Rows.Add(
                    row["OBJECT_NAME"].ToString(),
                    row["OBJECT_TYPE"].ToString()
                );
            }

        }

        private enum SystemCatalogEnum
        {
            Table,
            View,
            Procedure,
            Trigger,
            Sequence,
            Index,
            Function,
            Type,
            Lob,
            Package,
            Package_Body
            
        }

    }
}
