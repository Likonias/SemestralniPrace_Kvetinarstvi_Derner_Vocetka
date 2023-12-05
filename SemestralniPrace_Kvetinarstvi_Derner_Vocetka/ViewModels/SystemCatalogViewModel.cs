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
        public SystemCatalogViewModel()
        {
            dbUtil = new OracleDbUtil();
            InitializeTableData();
        }

        private async void InitializeTableData()
        {
            TableData = await GetSystemCatalogAsync();
        }

        public async Task<DataTable> GetSystemCatalogAsync()
        {
            //string commandText = "SELECT object_name, object_type FROM user_objects WHERE object_type IN ('TABLE', 'VIEW')";
            string commandText = "SELECT table_name, column_name, data_type, data_length\r\nFROM all_tab_columns";
            DataTable result = await dbUtil.ExecuteQueryAsync(commandText, null);

            //if (result.Rows.Count > 0)
            //{
            //    foreach (DataRow row in result.Rows)
            //    {
            //        DataClass data = new DataClass
            //        {
            //            ObjectName = row["OBJECT_NAME"].ToString(),
            //            ObjectType = row["OBJECT_TYPE"].ToString(),
            //        };
            //        Data.Add(data);
            //    }
            //}
            return result;
        }
    }
}
