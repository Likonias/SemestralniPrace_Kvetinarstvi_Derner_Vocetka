using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class FlowersViewModel : ViewModelBase
    {

        private OracleDbUtil dbUtil;
        private DataTable TableData { get; }

        public FlowersViewModel()
        {
            dbUtil = new OracleDbUtil();
            TableData = dbUtil.ExecuteQuery($"SELECT * FROM zakaznici");
        }
    }
}
