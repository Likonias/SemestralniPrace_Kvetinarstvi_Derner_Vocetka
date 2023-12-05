using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Repositories;
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
    public class OtherGoodsViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;
        private DataTable tableData;
        private OtherGoodsRepository otherGoodsRepository;
        private OtherGoodsStore otherGoodsStore;
        private INavigationService createOtherGoodsForm;

        public RelayCommand BtnAdd { get; }
        public RelayCommand BtnEdit { get; }
        public RelayCommand BtnDelete { get; }
        public DataRowView SelectedItem { get; set; }

        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }

        public OtherGoodsViewModel(INavigationService createOtherGoodsForm, OtherGoodsStore otherGoodsStore)
        {
            this.createOtherGoodsForm = createOtherGoodsForm;
            this.otherGoodsStore = otherGoodsStore;
            BtnAdd = new RelayCommand(BtnAddPressed);
            BtnEdit = new RelayCommand(BtnEditPressed);
            BtnDelete = new RelayCommand(BtnDeletePressed);
            dbUtil = new OracleDbUtil();
            tableData = new DataTable();
            otherGoodsRepository = new OtherGoodsRepository();
            InitializeTableData();
        }

        private async Task InitializeTableData()
        {
            TableData = new DataTable();
            TableData = await otherGoodsRepository.ConvertToDataTable();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private async void BtnDeletePressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                int otherGoodsIdToDelete = int.Parse(SelectedItem.Row[0].ToString());
                OtherGoods otherGoodsToDelete = await otherGoodsRepository.GetById(otherGoodsIdToDelete);
                await otherGoodsRepository.Delete(otherGoodsToDelete);
                InitializeTableData();
            }
        }

        private async void BtnEditPressed()
        {
            if (SelectedItem?.Row[0].ToString() != null)
            {
                otherGoodsStore.OtherGoods = await otherGoodsRepository.GetById(int.Parse(SelectedItem.Row[0].ToString()));
                createOtherGoodsForm.Navigate();
            }
        }

        private void BtnAddPressed()
        {
            otherGoodsStore.OtherGoods = null;
            createOtherGoodsForm.Navigate();
        }
    }
}
