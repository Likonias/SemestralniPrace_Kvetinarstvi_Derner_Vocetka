using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Entities;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class OccasionViewModel : ViewModelBase
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

        public OccasionViewModel()
        {
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData.Columns.Add("OccasionType");

            foreach (OccasionTypeEnum occasion in Enum.GetValues(typeof(OccasionTypeEnum)))
            {
                TableData.Rows.Add(occasion.ToString());
            }

        }

        public override void Dispose()
        {
            base.Dispose();
        }

    }
}
