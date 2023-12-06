using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class HistoryViewModel : ViewModelBase
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

        public HistoryViewModel()
        {
            dbUtil = new OracleDbUtil();

            InitializeTableData();
        }

        private async void InitializeTableData()
        {
            TableData = await dbUtil.ExecuteQueryAsync("SELECT * FROM DB_HISTORY", null);
        }
    }
}
