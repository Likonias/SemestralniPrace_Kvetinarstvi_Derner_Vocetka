using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Models;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Navigation.Stores;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Utils;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.ViewModels;
using SemestralniPrace_Kvetinarstvi_Derner_Vocetka.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;

namespace SemestralniPrace_Kvetinarstvi_Derner_Vocetka
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore navigationStore;
        private readonly ModalNavigationStore modalNavigationStore;
        private readonly AccountStore accountStore;
        private readonly AddressStore addressStore;
        private readonly CustomerStore customerStore;
        private readonly EmployeeStore employeeStore;
        private readonly OrderStore orderStore;
        private readonly OtherGoodsStore otherGoodsStore;
        private readonly FlowerStore flowerStore;
        private readonly AddressTypeStore addressTypeStore;
        private readonly BillingStore billingStore;
        private readonly DeliveryMethodStore deliveryMethodStore;
        private readonly DeliveryStore deliveryStore;
        private readonly InPersonPickupStore inPersonPickupStore;
        private readonly InvoiceStore invoiceStore;
        private readonly OccasionStore occasionStore;
        private readonly OrderStatusStore orderStatusStore;

        private readonly NavigationServiceManager serviceManager;

        private readonly LowStockLogChecker lowStockLogChecker;

        public App()
        {
            serviceManager = new NavigationServiceManager();
            flowerStore = new FlowerStore();
            // Register other navigation services here
            navigationStore = new NavigationStore();
            modalNavigationStore = new ModalNavigationStore();
            accountStore = new AccountStore();
            addressStore = new AddressStore();
            customerStore = new CustomerStore();
            employeeStore = new EmployeeStore();
            deliveryStore = new DeliveryStore();
            deliveryMethodStore = new DeliveryMethodStore();
            orderStore = new OrderStore();
            otherGoodsStore = new OtherGoodsStore();
            addressStore = new AddressStore();
            billingStore = new BillingStore();
            deliveryMethodStore = new DeliveryMethodStore();
            deliveryStore = new DeliveryStore();
            inPersonPickupStore = new InPersonPickupStore();
            invoiceStore = new InvoiceStore();
            occasionStore = new OccasionStore();
            orderStatusStore = new OrderStatusStore();

            CreateNavigationBarViewModel();

            lowStockLogChecker = new LowStockLogChecker(accountStore, CreateLowStockLogNavigationService());
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            INavigationService mainNavigationService = CreateMainNavigationService();
            mainNavigationService.Navigate();

            MainWindow = new MainWindow()
            {
                DataContext = new MainWindowViewModel(navigationStore, modalNavigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private INavigationService CreateMainNavigationService()
        {
            return new LayoutNavigationService<MainViewModel>(navigationStore, () => new MainViewModel(CreateLoginNavigationService()), CreateNavigationBarViewModel);
        }

        private INavigationService CreateLoginNavigationService()
        {
            return new ModalNavigationService<LoginViewModel>(modalNavigationStore, () => new LoginViewModel(accountStore, new CloseModalNavigationService(modalNavigationStore), CreateAccountNavigationService(), CreateLowStockLogNavigationService(), lowStockLogChecker));
        }

        private INavigationService CreateAccountNavigationService()
        {
            return new LayoutNavigationService<AccountViewModel>(navigationStore, () => new AccountViewModel(accountStore, CreateAccountNavigationService()), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersNavigationService()
        {
            return new LayoutNavigationService<FlowersViewModel>(navigationStore, () => new FlowersViewModel(CreateFlowersFormNavigationService(), flowerStore, accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateFlowersFormNavigationService() {
            return new ModalNavigationService<FlowerFormViewModel>(modalNavigationStore, () => new FlowerFormViewModel(CreateCloseModalNavigationService(), flowerStore, CreateFlowersNavigationService()));
        }

        private INavigationService CreateAddressesNavigationService()
        {
            return new LayoutNavigationService<AddressViewModel>(navigationStore, () => new AddressViewModel(CreateAddressesFormNavigationService(), addressStore, accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateAddressesFormNavigationService()
        {
            return new ModalNavigationService<AddressFormViewModel>(modalNavigationStore, () => new AddressFormViewModel(accountStore,CreateCloseModalNavigationService(), addressStore, CreateAddressesNavigationService()));
        }

        private INavigationService CreateCustomerNavigationService()
        {
            return new LayoutNavigationService<CustomerViewModel>(navigationStore, () => new CustomerViewModel(CreateCustomerFormNavigationService(), customerStore, accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateCustomerFormNavigationService()
        {
            return new ModalNavigationService<CustomerFormViewModel>(modalNavigationStore, () => new CustomerFormViewModel(customerStore, CreateCloseModalNavigationService(), CreateCustomerNavigationService(), accountStore));
        }

        private INavigationService CreateEmployeeNavigationService()
        {
            return new LayoutNavigationService<EmployeeViewModel>(navigationStore, () => new EmployeeViewModel(CreateEmployeeFormNavigationService(), CreateSupervisorNavigationService(), employeeStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateEmployeeFormNavigationService()
        {
            return new ModalNavigationService<EmployeeFormViewModel>(modalNavigationStore, () => new EmployeeFormViewModel(employeeStore, CreateCloseModalNavigationService(), CreateEmployeeNavigationService()));
        }

        private INavigationService CreateOrderNavigationService()
        {
            return new LayoutNavigationService<OrderViewModel>(navigationStore, () => new OrderViewModel(CreateOrderFormNavigationService(), orderStore, CreateOrderFlowerNavigationService(), CreateOrderOtherNavigationService(), CreateOrderFormNavigationService(), accountStore, CreateOrderNavigationService()), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOrderFormNavigationService()
        {
            return new ModalNavigationService<OrderFormViewModel>(modalNavigationStore, () => new OrderFormViewModel(orderStore, CreateCloseModalNavigationService(), CreateOrderFlowerNavigationService(), CreateOrderOtherNavigationService(), CreateOrderNavigationService()));
        }

        private INavigationService CreateOtherGoodsNavigationService()
        {
            return new LayoutNavigationService<OtherGoodsViewModel>(navigationStore, () => new OtherGoodsViewModel(CreateOtherGoodsFormNavigationService(), otherGoodsStore, accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOtherGoodsFormNavigationService()
        {
            return new ModalNavigationService<OtherGoodsFormViewModel>(modalNavigationStore, () => new OtherGoodsFormViewModel(CreateCloseModalNavigationService(), otherGoodsStore, CreateOtherGoodsNavigationService()));
        }

        private INavigationService CreateAddressTypeNavigationService()
        {
            return new LayoutNavigationService<AddressTypeViewModel>(navigationStore, () => new AddressTypeViewModel(addressTypeStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateBillingNavigationService()
        {
            return new LayoutNavigationService<BillingViewModel>(navigationStore, () => new BillingViewModel(CreateBillingFormNavigationService(), billingStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateBillingFormNavigationService()
        {
            return new ModalNavigationService<BillingFormViewModel>(modalNavigationStore, () => new BillingFormViewModel(CreateCloseModalNavigationService(), billingStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateDeliveryMethodNavigationService()
        {
            return new LayoutNavigationService<DeliveryMethodViewModel>(navigationStore, () => new DeliveryMethodViewModel(CreateDeliveryMethodFormNavigationService(), deliveryMethodStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateDeliveryMethodFormNavigationService()
        {
            return new ModalNavigationService<DeliveryMethodFormViewModel>(modalNavigationStore, () => new DeliveryMethodFormViewModel(CreateCloseModalNavigationService(), deliveryMethodStore, CreateDeliveryMethodNavigationService()));
        }

        private INavigationService CreateDeliveryNavigationService()
        {
            return new LayoutNavigationService<DeliveryViewModel>(navigationStore, () => new DeliveryViewModel(CreateDeliveryFormNavigationService(),deliveryStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateDeliveryFormNavigationService()
        {
            return new ModalNavigationService<DeliveryFormViewModel>(modalNavigationStore, () => new DeliveryFormViewModel(CreateDeliveryFormNavigationService(), deliveryStore, CreateFlowersNavigationService()));
        }

        private INavigationService CreateInPersonPickupNavigationService()
        {
            return new LayoutNavigationService<InPersonPickupViewModel>(navigationStore, () => new InPersonPickupViewModel(CreateInPersonPicupFormNavigationService(), inPersonPickupStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateInPersonPicupFormNavigationService()
        {
            return new ModalNavigationService<InPersonPickupFormViewModel>(modalNavigationStore, () => new InPersonPickupFormViewModel( CreateCloseModalNavigationService(), inPersonPickupStore, CreateInPersonPickupNavigationService()));
        }

        private INavigationService CreateInvoiceNavigationService()
        {
            return new LayoutNavigationService<InvoiceViewModel>(navigationStore, () => new InvoiceViewModel(CreateInvoiceFormNavigationService(), invoiceStore, accountStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateInvoiceFormNavigationService()
        {
            return new ModalNavigationService<InvoiceFormViewModel>(modalNavigationStore, () => new InvoiceFormViewModel(CreateCloseModalNavigationService(), invoiceStore, CreateInvoiceNavigationService()));
        }

        private INavigationService CreateOccasionNavigationService()
        {
            return new LayoutNavigationService<OccasionViewModel>(navigationStore, () => new OccasionViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOccasionFormNavigationService()
        {
            return new ModalNavigationService<OccasionFormViewModel>(modalNavigationStore, () => new OccasionFormViewModel( CreateCloseModalNavigationService(), occasionStore, CreateOccasionNavigationService()));
        }

        private INavigationService CreateOrderStatusNavigationService()
        {
            return new LayoutNavigationService<OrderStatusViewModel>(navigationStore, () => new OrderStatusViewModel(CreateOrderStatusFormNavigationService(), orderStatusStore), CreateNavigationBarViewModel);
        }

        private INavigationService CreateOrderStatusFormNavigationService()
        {
            return new ModalNavigationService<OrderStatusFormViewModel>(modalNavigationStore, () => new OrderStatusFormViewModel(CreateCloseModalNavigationService(), orderStatusStore, CreateOrderStatusNavigationService()));
        }

        private INavigationService CreateOrderOtherNavigationService()
        {
            return new ModalNavigationService<OrderOtherViewModel>(modalNavigationStore, () => new OrderOtherViewModel(orderStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateOrderFlowerNavigationService()
        {
            return new ModalNavigationService<OrderFlowerViewModel>(modalNavigationStore, () => new OrderFlowerViewModel(orderStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateSupervisorNavigationService() {
            return new ModalNavigationService<SupervisorViewModel>(modalNavigationStore, () => new SupervisorViewModel(employeeStore, CreateCloseModalNavigationService()));
        }

        private INavigationService CreateSystemCatalogNavigationService()
        {
            return new LayoutNavigationService<SystemCatalogViewModel>(navigationStore, () => new SystemCatalogViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateUserOtherNavigationService()
        {
            return new LayoutNavigationService<UserOtherViewModel>(navigationStore, () => new UserOtherViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateUserFlowerNavigationService()
        {
            return new LayoutNavigationService<UserFlowerViewModel>(navigationStore, () => new UserFlowerViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateLowStockLogNavigationService()
        {
            return new ModalNavigationService<LowStockLogViewModel>(modalNavigationStore, () => new LowStockLogViewModel(accountStore, CreateCloseModalNavigationService(), lowStockLogChecker));
        }

        private NavigationBarViewModel CreateNavigationBarViewModel()
        {
            UpdateServiceManager();
            return new NavigationBarViewModel(accountStore, serviceManager);
        }
        private INavigationService CreateHistoryNavigationService()
        {
            return new LayoutNavigationService<HistoryViewModel>(navigationStore, () => new HistoryViewModel(), CreateNavigationBarViewModel);
        }

        private INavigationService CreateCloseModalNavigationService()
        {
            return new CloseModalNavigationService(modalNavigationStore);
        }


        private void UpdateServiceManager()
        {
            serviceManager.ClearNavigationService();
            serviceManager.RegisterNavigationService<LoginViewModel>(CreateLoginNavigationService());
            serviceManager.RegisterNavigationService<MainViewModel>(CreateMainNavigationService());
            serviceManager.RegisterNavigationService<AccountViewModel>(CreateAccountNavigationService());
            serviceManager.RegisterNavigationService<FlowersViewModel>(CreateFlowersNavigationService());
            serviceManager.RegisterNavigationService<FlowerFormViewModel>(CreateFlowersFormNavigationService());
            serviceManager.RegisterNavigationService<AddressViewModel>(CreateAddressesNavigationService());
            serviceManager.RegisterNavigationService<CustomerViewModel>(CreateCustomerNavigationService());
            serviceManager.RegisterNavigationService<CustomerFormViewModel>(CreateCustomerFormNavigationService());
            serviceManager.RegisterNavigationService<EmployeeViewModel>(CreateEmployeeNavigationService());
            serviceManager.RegisterNavigationService<EmployeeFormViewModel>(CreateEmployeeFormNavigationService());
            serviceManager.RegisterNavigationService<OrderViewModel>(CreateOrderNavigationService());
            serviceManager.RegisterNavigationService<OrderFormViewModel>(CreateOrderFormNavigationService());
            serviceManager.RegisterNavigationService<OtherGoodsViewModel>(CreateOtherGoodsNavigationService());
            serviceManager.RegisterNavigationService<OtherGoodsFormViewModel>(CreateOtherGoodsFormNavigationService());
            serviceManager.RegisterNavigationService<AddressTypeViewModel>(CreateAddressTypeNavigationService());
            serviceManager.RegisterNavigationService<BillingViewModel>(CreateBillingNavigationService());
            serviceManager.RegisterNavigationService<BillingFormViewModel>(CreateBillingFormNavigationService());
            serviceManager.RegisterNavigationService<DeliveryMethodViewModel>(CreateDeliveryMethodNavigationService());
            serviceManager.RegisterNavigationService<DeliveryMethodFormViewModel>(CreateDeliveryMethodFormNavigationService());
            serviceManager.RegisterNavigationService<DeliveryViewModel>(CreateDeliveryNavigationService());
            serviceManager.RegisterNavigationService<DeliveryFormViewModel>(CreateDeliveryFormNavigationService());
            serviceManager.RegisterNavigationService<InPersonPickupViewModel>(CreateInPersonPickupNavigationService());
            serviceManager.RegisterNavigationService<InPersonPickupFormViewModel>(CreateInPersonPicupFormNavigationService());
            serviceManager.RegisterNavigationService<InvoiceViewModel>(CreateInvoiceNavigationService());
            serviceManager.RegisterNavigationService<InvoiceFormViewModel>(CreateInvoiceFormNavigationService());
            serviceManager.RegisterNavigationService<OccasionViewModel>(CreateOccasionNavigationService());
            serviceManager.RegisterNavigationService<OccasionFormViewModel>(CreateOccasionFormNavigationService());
            serviceManager.RegisterNavigationService<OrderStatusViewModel>(CreateOrderStatusNavigationService());
            serviceManager.RegisterNavigationService<OrderStatusFormViewModel>(CreateOrderStatusFormNavigationService());
            serviceManager.RegisterNavigationService<SystemCatalogViewModel>(CreateSystemCatalogNavigationService());
            serviceManager.RegisterNavigationService<HistoryViewModel>(CreateHistoryNavigationService());
            serviceManager.RegisterNavigationService<UserOtherViewModel>(CreateUserOtherNavigationService());
            serviceManager.RegisterNavigationService<UserFlowerViewModel>(CreateUserFlowerNavigationService());
        }

    }
}
