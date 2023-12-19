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
    public class UserOtherViewModel : ViewModelBase
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

        public UserOtherViewModel()
        {
            this.dbUtil = new OracleDbUtil();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            DataTable dt = await dbUtil.LoadDataFromViewAsync("ostatni_view");
            
            TableData = new DataTable();

            TableData.Columns.Add("Name");
            TableData.Columns.Add("Price");
            TableData.Columns.Add("Warehouse");
            TableData.Columns.Add("CountryOfOrigin");
            TableData.Columns.Add("ExpirationDate");
            TableData.Columns.Add("Image", typeof(byte[]));

            foreach (DataRow row in dt.Rows)
            {
                byte[] imageBytes = row["OBRAZEK"] as byte[] ?? new byte[0];

                TableData.Rows.Add(
                    row["NAZEV"].ToString(),
                    Convert.ToInt32(row["CENA"]),
                    row["SKLAD"].ToString(),
                    row["ZEME_PUVODU"].ToString(),
                    row["DATUM_TRVANLIVOSTI"].ToString(),
                    imageBytes
                );
            }
        }
    }
}
