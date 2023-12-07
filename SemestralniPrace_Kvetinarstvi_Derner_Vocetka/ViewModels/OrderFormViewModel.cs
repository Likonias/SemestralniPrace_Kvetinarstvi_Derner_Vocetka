using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
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
    public class OrderFormViewModel : ViewModelBase
    {
        private OrderStore orderStore; 
        private DataTable tableData;
        private INavigationService closeNavigationService;
        private INavigationService createOrderFlower;
        private INavigationService createOrderOther;
        public DataTable TableData
        {
            get { return tableData; }
            set
            {
                tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        public List<OccasionTypeEnum> OccasionComboBoxItems { get; }
        public List<BillingTypeEnum> BillingComboBoxItems { get; }
        public List<string> GoodsComboBoxItems { get; }
        public List<int> GoodsId { get; }
        public List<int> GoodsCount { get; }

        public RelayCommand BtnOk { get; }
        public RelayCommand BtnCancel { get; }

        public OrderFormViewModel(OrderStore orderStore, INavigationService closeNavigationService, INavigationService createOrderFlower, INavigationService createOrderOther)
        {
            this.orderStore = orderStore;
            this.closeNavigationService = closeNavigationService;
            this.createOrderFlower = createOrderFlower;
            this.createOrderOther = createOrderOther;

            OccasionComboBoxItems = Enum.GetValues(typeof(OccasionTypeEnum)).Cast<OccasionTypeEnum>().ToList();
            BillingComboBoxItems = Enum.GetValues(typeof(BillingTypeEnum)).Cast<BillingTypeEnum>().ToList();

            GoodsComboBoxItems = new List<string>();
            GoodsId = new List<int>();
            GoodsCount = new List<int>();

            BtnOk = new RelayCommand(BtnOkClicked);
            BtnCancel = new RelayCommand(BtnCancelClicked);

            LoadGoodsFromDatabase(); // Call method to load goods from SQL
        }

        private void BtnCancelClicked()
        {
            throw new NotImplementedException();
        }

        private void BtnOkClicked()
        {
            throw new NotImplementedException();
        }

        private void LoadGoodsFromDatabase()
        {
            //// Placeholder SQL query
            //string query = "SELECT Id, Name FROM GoodsTable"; // Modify to your table name and structure

            //// Execute the query and fetch data
            //// Assuming you have a database connection or a service to execute queries
            //// Replace this with your actual database handling code
            //var goodsData = YourDatabaseService.ExecuteQuery(query);

            //GoodsComboBoxItems = goodsData.Select(item => item["Name"].ToString()).ToList();
            //GoodsId = goodsData.Select(item => Convert.ToInt32(item["Id"])).ToList();
            //GoodsCount = new List<int>(GoodsId.Count); // Initializing an empty list for counts
        }

    }
}
