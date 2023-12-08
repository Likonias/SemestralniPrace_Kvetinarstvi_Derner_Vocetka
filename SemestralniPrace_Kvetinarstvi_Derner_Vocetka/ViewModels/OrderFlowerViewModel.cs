using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
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
    public class OrderFlowerViewModel : ViewModelBase
    {
        public RelayCommand BtnClose { get; }
        private DataTable tableData;
        private OracleDbUtil dbUtil;
        private OrderStore orderStore;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public OrderFlowerViewModel(OrderStore orderStore, INavigationService closeNavigationService)
        {
            this.orderStore = orderStore;
            CloseNavigationService = closeNavigationService;
            BtnClose = new RelayCommand(BtnCloseClicked);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            DataTable dt = await dbUtil.ExecuteGetGoodsFunctionAsync("GetZboziByObjednavkaId", orderStore.Id, "K");

            TableData = new DataTable();

            TableData.Columns.Add("Nazev");
            TableData.Columns.Add("Cena");
            TableData.Columns.Add("Sklad");
            TableData.Columns.Add("Stav");
            TableData.Columns.Add("Stáří");

            foreach (DataRow row in dt.Rows)
            {
                TableData.Rows.Add(
                    row["NAZEV"].ToString(),
                    Convert.ToInt32(row["CENA"]),
                    row["SKLAD"].ToString(),
                    row["STAV"].ToString(),
                    Convert.ToInt32(row["STARI"])
                );
            }

            
        }

        private void BtnCloseClicked()
        {
            CloseNavigationService.Navigate();
        }
        public INavigationService CloseNavigationService { get; }
    }
}
