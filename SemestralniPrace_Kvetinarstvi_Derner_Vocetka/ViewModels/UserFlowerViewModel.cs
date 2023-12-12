using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels
{
    public class UserFlowerViewModel : ViewModelBase
    {
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
        private OracleDbUtil dbUtil;

        public UserFlowerViewModel()
        {
            this.dbUtil = new OracleDbUtil();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            DataTable dt = await dbUtil.LoadDataFromViewAsync("kytky_view");

            TableData = new DataTable();
            
            TableData.Columns.Add("Name", typeof(string));
            TableData.Columns.Add("Price", typeof(int));
            TableData.Columns.Add("Warehouse", typeof(int));
            TableData.Columns.Add("FlowerState", typeof(string));
            TableData.Columns.Add("Age", typeof(int));
            TableData.Columns.Add("Image", typeof(byte[]));

            foreach (DataRow row in dt.Rows)
            {
                byte[] imageBytes = row["OBRAZEK"] as byte[] ?? new byte[0];

                TableData.Rows.Add(
                    row["NAZEV"].ToString(),
                    Convert.ToInt32(row["CENA"]),
                    row["SKLAD"].ToString(),
                    row["STAV"].ToString(),
                    row["STARI"].ToString(),
                    imageBytes
                );
            }
        }
    }
}
