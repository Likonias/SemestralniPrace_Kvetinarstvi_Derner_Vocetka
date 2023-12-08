using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models.Enums;
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
    public class OrderFormViewModel : ViewModelBase
    {
        private OracleDbUtil dbUtil;
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
        public List<string> GoodsComboBoxItems { get; set; }
        public List<int> GoodsId { get; set; }
        public List<int> GoodsCount { get; set; }
        private List<int> GoodsIdInOrder { get; set; }
        private List<int> GoodsCountInOrder { get; set; }
        public List<string> DeliveryComboBoxItems { get; }
        public List<string> DeliveryCompanyComboBoxItems { get; }
        public List<string> CustomerComboBoxItems { get; }
        public RelayCommand BtnOk { get; }
        public RelayCommand BtnCancel { get; }
        public RelayCommand BtnAddToOrder { get; }
        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        private bool isDeliverySelected;
        public bool IsDeliverySelected
        {
            get { return isDeliverySelected; }
            set
            {
                isDeliverySelected = value;
                OnPropertyChanged("IsDeliverySelected");
            }
        }
        private bool isEmployee;
        public bool IsEmployee
        {
            get { return isEmployee; }
            set
            {
                isEmployee = value;
                OnPropertyChanged(nameof(IsEmployee));
            }
        }
        private DataTable dataTable;
        public OrderFormViewModel(OrderStore orderStore, INavigationService closeNavigationService, INavigationService createOrderFlower, INavigationService createOrderOther)
        {
            this.orderStore = orderStore;
            this.closeNavigationService = closeNavigationService;
            this.createOrderFlower = createOrderFlower;
            this.createOrderOther = createOrderOther;
            dbUtil = new OracleDbUtil();
            TableData = new DataTable();
            
            dataTable = new DataTable();

            dataTable.Columns.Add("ID_ZBOZI");
            dataTable.Columns.Add("NAZEV");
            dataTable.Columns.Add("SKLAD");

            OccasionComboBoxItems = Enum.GetValues(typeof(OccasionTypeEnum)).Cast<OccasionTypeEnum>().ToList();
            BillingComboBoxItems = Enum.GetValues(typeof(BillingTypeEnum)).Cast<BillingTypeEnum>().ToList();

            GoodsComboBoxItems = new List<string>();
            GoodsId = new List<int>();
            GoodsCount = new List<int>();
            GoodsIdInOrder = new List<int>();
            GoodsCountInOrder = new List<int>();
            DeliveryComboBoxItems = new List<string>
            {
                "In Person",
                "Delivery Company"
            };
            IsDeliverySelected = false;
            DeliveryCompanyComboBoxItems = new List<string>
            {
                "PPL",
                "DHL",
                "CeskaPosta"
            };
            BtnAddToOrder = new RelayCommand(BtnAddToOrderClicked);
            BtnOk = new RelayCommand(BtnOkClicked);
            BtnCancel = new RelayCommand(BtnCancelClicked);

            IsEmployee = !orderStore.IsCustomer;
            CustomerComboBoxItems = new List<string>();
            InitializeCustomerComboBox();
            LoadGoodsFromDatabase();
        }

        private async void InitializeCustomerComboBox()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            await customerRepository.GetAll();
            
            foreach (Customer customer in customerRepository.Customers) 
            {
                CustomerComboBoxItems.Add(customer.Email);
            }

        }

        private void BtnAddToOrderClicked()
        {
            if (int.TryParse(Count, out int enteredCount))
            {
                int selectedGoodsIndex = GoodsComboBoxItems.IndexOf(SelectedGoods);
                if (selectedGoodsIndex >= 0 && selectedGoodsIndex < GoodsCount.Count)
                {
                    if (enteredCount <= GoodsCount[selectedGoodsIndex])
                    {
                        int existingIndex = GoodsIdInOrder.IndexOf(GoodsId[selectedGoodsIndex]);

                        if (existingIndex >= 0)
                        {
                            GoodsCountInOrder[existingIndex] += enteredCount;
                        }
                        else
                        {
                            GoodsIdInOrder.Add(GoodsId[selectedGoodsIndex]);
                            GoodsCountInOrder.Add(enteredCount);
                        }
                        GoodsCount[selectedGoodsIndex] -= enteredCount;

                        dataTable.Rows.Add(
                            GoodsId[selectedGoodsIndex],
                            GoodsComboBoxItems[selectedGoodsIndex],
                            enteredCount
                        );
                        TableData = dataTable;
                        
                    }
                }
            }
        }

        private void BtnCancelClicked()
        {
            closeNavigationService.Navigate();
        }

        private async void BtnOkClicked()
        {
            Account cusAcc;
            int? emplId = null;
            int cusId;
            char zpusobPrevzeti = 'O';
            if (DeliveryComboBoxItems.IndexOf(SelectedDelivery) == 1)
            {
                zpusobPrevzeti = 'D';
            }
            if (orderStore.IsCustomer)
            {
                cusId = orderStore.IdAccount;
            }
            else
            {
                cusAcc = await dbUtil.ExecuteGetAccountFunctionAsync("getuserbyemail", SelectedCustomer);
                cusId = cusAcc.Id;
                emplId = orderStore.IdAccount;
            }
            var parameters = new Dictionary<string, object>
            {
                { "p_zakaznici_id", cusId },
                { "p_zamestnanci_id", emplId },
                { "p_zpusob_prevzeti_typ", zpusobPrevzeti },
                { "ZBOZI_IDS", string.Join(",", GoodsIdInOrder) },
                { "ZBOZI_POCET", string.Join(",", GoodsCountInOrder) },
                { "p_prilezitost", SelectedOccasion.ToString() },
                { "p_druh_platby", BillingComboBoxItems.IndexOf(SelectedBilling) + 1 },
                { "p_spolecnost", SelectedDeliveryCompany }
            };

            await dbUtil.ExecuteStoredProcedureAsync("create_objednavka", parameters);

            closeNavigationService.Navigate();
        }

        private async void LoadGoodsFromDatabase()
        {
            // Placeholder SQL query
            string query = "SELECT ID_ZBOZI, NAZEV, SKLAD FROM ZBOZI"; // Modify to your table name and structure

            DataTable dt = await dbUtil.ExecuteQueryAsync(query);
            
            //GoodsComboBoxItems = dt.AsEnumerable().Select(row => row.Field<string>("NAZEV")).ToList();
            //GoodsId = dt.AsEnumerable().Select(row => Convert.ToInt32(row.Field<decimal>("ID_ZBOZI"))).ToList();
            //GoodsCount = dt.AsEnumerable().Select(row => Convert.ToInt32(row.Field<decimal>("SKLAD"))).ToList();
            foreach (DataRow row in dt.Rows)
            {
                GoodsId.Add(Convert.ToInt32(row["ID_ZBOZI"]));
                GoodsComboBoxItems.Add(row["NAZEV"].ToString());
                GoodsCount.Add(Convert.ToInt32(row["SKLAD"]));
            }

        }

        private OccasionTypeEnum _selectedOccasion;
        public OccasionTypeEnum SelectedOccasion
        {
            get => _selectedOccasion;
            set
            {
                _selectedOccasion = value;
                OnPropertyChanged(nameof(SelectedOccasion));
            }
        }

        private BillingTypeEnum _selectedBilling;
        public BillingTypeEnum SelectedBilling
        {
            get => _selectedBilling;
            set
            {
                _selectedBilling = value;
                OnPropertyChanged(nameof(SelectedBilling));
            }
        }

        private string _selectedCustomer;
        public string SelectedCustomer
        {
            get => _selectedCustomer;
            set
            {
                _selectedCustomer = value;
                OnPropertyChanged(nameof(SelectedCustomer));
            }
        }
        
        private string _selectedDelivery;
        public string SelectedDelivery
        {
            get => _selectedDelivery;
            set
            {
                _selectedDelivery = value;
                if(value == "Delivery Company")
                {
                    IsDeliverySelected = true;
                }
                OnPropertyChanged(nameof(SelectedDelivery));
            }
        }

        private string _selectedDeliveryCompany;
        public string SelectedDeliveryCompany
        {
            get => _selectedDeliveryCompany;
            set
            {
                _selectedDeliveryCompany = value;
                OnPropertyChanged(nameof(SelectedDeliveryCompany));
            }
        }

        private string _selectedGoods;
        public string SelectedGoods
        {
            get => _selectedGoods;
            set
            {
                _selectedGoods = value;
                OnPropertyChanged(nameof(SelectedGoods));
            }
        }

        private string _count;
        public string Count
        {
            get => _count;
            set 
            { 
                _count = value;
                if (!IsCountLowerThanSelectedGoodsCount() && Count != "")
                {
                    ErrorMessage = "Not enough stock!";
                }
                else
                {
                    ErrorMessage = "";
                }
                OnPropertyChanged(nameof(Count));
            }
        }

        public bool IsCountLowerThanSelectedGoodsCount()
        {
            if (int.TryParse(Count, out int enteredCount))
            {
                // Assuming GoodsCount and GoodsComboBoxItems are aligned by index
                int selectedGoodsIndex = GoodsComboBoxItems.IndexOf(SelectedGoods);
                if (selectedGoodsIndex >= 0 && selectedGoodsIndex < GoodsCount.Count)
                {
                    int selectedGoodsCount = GoodsCount[selectedGoodsIndex];
                    return enteredCount < selectedGoodsCount;
                }
            }
            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
